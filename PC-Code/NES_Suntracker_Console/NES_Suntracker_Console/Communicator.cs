using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NES_Suntracker_Console
{
    class Communicator
    {
        private static Communicator instance = null;
        SerialPort serialPort_actuator;
        SerialPort serialPort_masternode;

        private bool trackerOverride = false;
        private DateTime lastUpdated = new DateTime();
        private Int16[] solarIntensities = new Int16[4];
        private Int16 sunAngle;
        private double solarVoltage;

        internal DateTime GetLastUpdateTime()
        {
            return lastUpdated;
        }
        internal double GetSolarvoltage()
        {
            return solarVoltage;
        }
        internal Int16 GetSunangle()
        {
            return sunAngle;
        }
        internal Int16[] GetSolarIntensities()
        {
            return solarIntensities;
        }
        internal void SetTrackerOverride(bool state)
        {
            trackerOverride = state;
        }
        private Communicator()
        {
            //-----------------------
            // Actuator link
            // ---------------------
            serialPort_actuator = new SerialPort();
            serialPort_actuator.PortName = "COM6";
            serialPort_actuator.DataBits = 8;
            serialPort_actuator.Parity = Parity.None;
            serialPort_actuator.StopBits = StopBits.One;
            serialPort_actuator.BaudRate = 8196;
            serialPort_actuator.ReadTimeout = 100;
            serialPort_actuator.DataReceived += serialPort_DataReceived;
            serialPort_actuator.Open();
            initMotor();

            //------------------------
            // Masternode link
            //------------------------
            serialPort_masternode = new SerialPort();
            serialPort_masternode.PortName = "COM7";
            serialPort_masternode.DataBits = 8;
            serialPort_masternode.Parity = Parity.None;
            serialPort_masternode.StopBits = StopBits.One;
            serialPort_masternode.BaudRate = 115200;
            serialPort_masternode.ReadTimeout = 100;
            serialPort_masternode.DataReceived += serialPort_masternode_DataReceived;
            serialPort_masternode.Open();
        }

        void serialPort_masternode_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort_masternode.DataReceived -= serialPort_masternode_DataReceived;
            byte[] recvBuffer = new byte[15];

            serialPort_masternode.Read(recvBuffer, 0, 15);
            lastUpdated = DateTime.Now;

            //Bad coding style but time is a scare ressource...
            if (recvBuffer[0] == 0xee)
            {
                byte[] receivedAngle = new byte[2];
                receivedAngle[0] = recvBuffer[1];
                receivedAngle[1] = recvBuffer[2];
                sunAngle = BitConverter.ToInt16(receivedAngle, 0);

                if (!trackerOverride)
                    SendActuatorPosition(sunAngle);
            }
            else
                sunAngle = -1; // Error inducator

            if (recvBuffer[3] == 0xdd)
            {
                byte[] intens1 = new byte[2];
                intens1[0] = recvBuffer[4];
                intens1[1] = recvBuffer[5];
                solarIntensities[0] = BitConverter.ToInt16(intens1, 0);

                byte[] intens2 = new byte[2];
                intens2[0] = recvBuffer[6];
                intens2[1] = recvBuffer[7];
                solarIntensities[1] = BitConverter.ToInt16(intens2, 0);

                byte[] intens3 = new byte[2];
                intens3[0] = recvBuffer[8];
                intens3[1] = recvBuffer[9];
                solarIntensities[2] = BitConverter.ToInt16(intens3, 0);

                byte[] intens4 = new byte[2];
                intens4[0] = recvBuffer[10];
                intens4[1] = recvBuffer[11];
                solarIntensities[3] = BitConverter.ToInt16(intens4, 0);
            }
            else
            {
                //Error indicator
                for (int i = 0; i < 4; i++)
                    solarIntensities[i] = -1;
            }
            if (recvBuffer[12] == 0xcc)
            {
                double vSteps = 1.5/4095; // ADC voltage step
                byte[] solarV = new byte[2];
                solarV[0] = recvBuffer[13];
                solarV[1] = recvBuffer[14];
                solarVoltage = Math.Round(BitConverter.ToInt16(solarV,0) * vSteps,3);
            }
            else
                solarVoltage = -1;

            serialPort_masternode.DiscardInBuffer();
            serialPort_masternode.DataReceived += serialPort_masternode_DataReceived;
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort_actuator.DiscardInBuffer();
        }
        private byte[] appendChecksum(List<byte> input)
        {
            int sum = 0;
            foreach (byte b in input)
                sum += b;
            byte chksum = BitConverter.GetBytes(sum)[0];
            input.Add((byte)~chksum); //append chksum
            input.Insert(0, 0xff); // add initializator
            input.Insert(0, 0xff);
            return input.ToArray<byte>();
        }
        internal static Communicator GetInstance()
        {
            if (instance == null)
                instance = new Communicator();
            return instance;
        }
        internal void SendActuatorPosition(Int16 angle)
        {
            List<byte> command = new List<byte>();
            command.Add(0x01); // ID
            command.Add(0x05); // length: 2 + 3params + regadr
            command.Add(0x03); // write cmd
            command.Add(0x1e); // reg adr

            byte[] angleAsBytes = BitConverter.GetBytes((int)(angle/0.29));

            command.Add(angleAsBytes[0]);
            command.Add(angleAsBytes[1]);

            byte[] commandToSend = appendChecksum(command);
            serialPort_actuator.Write(commandToSend, 0, commandToSend.Length);
            
        }
        internal int getTemperature()
        {
            serialPort_actuator.DataReceived -= serialPort_DataReceived;
            List<int> result = new List<int>();
            try
            {
                List<byte> command = new List<byte>();
                command.Add(0x01); // ID
                command.Add(0x04); // length: 2 + 3params + regadr
                command.Add(0x02); // read cmd
                command.Add(0x2b); // reg adr
                command.Add(0x01);


                byte[] commandToSend = appendChecksum(command);

                serialPort_actuator.Write(commandToSend, 0, commandToSend.Length);
                System.Threading.Thread.Sleep(100);
                for (int i = 0; i < 7; i++)
                    result.Add(serialPort_actuator.ReadByte());
            }
            catch
            {
                for (int i = 0; i < 10; i++)
                    result.Add(0);
            }
            finally
            {
                serialPort_actuator.DataReceived += serialPort_DataReceived;
            }
            return result[5];
        }
        internal int getVoltage()
        {
            serialPort_actuator.DataReceived -= serialPort_DataReceived;
            List<byte> command = new List<byte>();
            List<int> result = new List<int>();
            command.Add(0x01); // ID
            command.Add(0x04); // length: 2 + 3params + regadr
            command.Add(0x02); // read cmd
            command.Add(0x2a); // reg adr
            command.Add(0x01);

            try
            {
                byte[] commandToSend = appendChecksum(command);

                serialPort_actuator.Write(commandToSend, 0, commandToSend.Length);
                System.Threading.Thread.Sleep(100);

                for (int i = 0; i < 7; i++)
                    result.Add(serialPort_actuator.ReadByte());
            }
            catch
            {
                for (int i = 0; i < 10; i++)
                    result.Add(10);
            }
            finally
            {
                serialPort_actuator.DataReceived += serialPort_DataReceived;
            }

            return result[5]/10;
        }
        internal double getPosition()
        {
            List<byte> command = new List<byte>();
            List<int> result = new List<int>();
            serialPort_actuator.DataReceived -= serialPort_DataReceived;
            int upper = 0;
            int lower = 0;
            try
            {
                command.Add(0x01); // ID
                command.Add(0x04); // length: 2 + 3params + regadr
                command.Add(0x02); // read cmd
                command.Add(0x24); // reg adr
                command.Add(0x02);


                byte[] commandToSend = appendChecksum(command);

                serialPort_actuator.Write(commandToSend, 0, commandToSend.Length);
                System.Threading.Thread.Sleep(100);

                for (int i = 0; i < 8; i++)
                    result.Add(serialPort_actuator.ReadByte());

                lower = result[5];
                upper = result[6];
            }
            catch
            {
            }
            finally
            {
                serialPort_actuator.DataReceived += serialPort_DataReceived;
            }

            return (upper << 8 | lower) * 0.29;
        }
        private void initMotor()
        {
            List<byte> command = new List<byte>();
            command.Add(0x01); // ID
            command.Add(0x05); // length: 2 + 3params + regadr
            command.Add(0x03); // write cmd
            command.Add(0x18); // reg adr
            command.Add(0x01);
            command.Add(0x01);

            byte[] commandToSend = appendChecksum(command);
            serialPort_actuator.Write(commandToSend, 0, commandToSend.Length);
        }
    }
}
