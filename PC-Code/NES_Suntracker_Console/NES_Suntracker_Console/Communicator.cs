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

        private bool trackerOverride = true;
        private DateTime[] lastUpdated = new DateTime[4];
        private Int16[] solarIntensities = new Int16[4];
        private Int16 sunAngle;
        private double solarVoltage;

        internal DateTime[] GetLastUpdateTime()
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

            byte[] firstReceived = new byte[1];
            serialPort_masternode.Read(firstReceived, 0, 1);

            if(firstReceived[0] == 0xee) // sun data
            {
                byte[] recvBuffer = new byte[6];
                serialPort_masternode.Read(recvBuffer, 0, 6);

                byte[] receivedNodeAdr = new byte[2];
                receivedNodeAdr[0] = recvBuffer[0];
                receivedNodeAdr[1] = recvBuffer[1];
                Int16 nodeID = BitConverter.ToInt16(receivedNodeAdr, 0);
                lastUpdated[nodeID] = DateTime.Now;

                byte[] intens = new byte[2];
                intens[0] = recvBuffer[2];
                intens[1] = recvBuffer[3];
                solarIntensities[nodeID] = BitConverter.ToInt16(intens, 0);

                byte[] recvAngle = new byte[2];
                recvAngle[0] = recvBuffer[4];
                recvAngle[1] = recvBuffer[5];
                sunAngle = BitConverter.ToInt16(recvAngle, 0);
            }
            else if(firstReceived[0] == 0xdd) // ADC val
            {
                byte[] recvBuffer = new byte[2];
                serialPort_masternode.Read(recvBuffer, 0, 2);
                double vSteps = 1.5 / 4095; // ADC voltage step
                solarVoltage = Math.Round(BitConverter.ToInt16(recvBuffer, 0) * vSteps, 3);
            }

            serialPort_masternode.DiscardInBuffer(); // clean up whatever errorneus data is still in here
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
