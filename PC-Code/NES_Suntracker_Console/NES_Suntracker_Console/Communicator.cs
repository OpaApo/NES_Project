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
        SerialPort serialPort;
        private Communicator()
        {
            serialPort = new SerialPort();
            serialPort.PortName = "COM6";
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.BaudRate = 8196;
            serialPort.ReadTimeout = 100;
            serialPort.DataReceived += serialPort_DataReceived;
            serialPort.Open();
            initMotor();
        }

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort.DiscardInBuffer();
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
            serialPort.Write(commandToSend, 0, commandToSend.Length);
            
        }
        internal int getTemperture()
        {
            List<byte> command = new List<byte>();
            command.Add(0x01); // ID
            command.Add(0x04); // length: 2 + 3params + regadr
            command.Add(0x02); // read cmd
            command.Add(0x2b); // reg adr
            command.Add(0x01);


            byte[] commandToSend = appendChecksum(command);
            serialPort.DataReceived -= serialPort_DataReceived;
            serialPort.Write(commandToSend, 0, commandToSend.Length);
            System.Threading.Thread.Sleep(100);
            List<int> result = new List<int>();
            for (int i = 0; i < 7; i++)
                result.Add(serialPort.ReadByte());

            serialPort.DataReceived += serialPort_DataReceived;

            return result[5];
        }
        internal int getVoltage()
        {
            List<byte> command = new List<byte>();
            command.Add(0x01); // ID
            command.Add(0x04); // length: 2 + 3params + regadr
            command.Add(0x02); // read cmd
            command.Add(0x2a); // reg adr
            command.Add(0x01);


            byte[] commandToSend = appendChecksum(command);
            serialPort.DataReceived -= serialPort_DataReceived;
            serialPort.Write(commandToSend, 0, commandToSend.Length);
            System.Threading.Thread.Sleep(100);
            List<int> result = new List<int>();
            for (int i = 0; i < 7; i++)
                result.Add(serialPort.ReadByte());

            serialPort.DataReceived += serialPort_DataReceived;

            return result[5]/10;
        }
        internal double getPosition()
        {
            List<byte> command = new List<byte>();
            command.Add(0x01); // ID
            command.Add(0x04); // length: 2 + 3params + regadr
            command.Add(0x02); // read cmd
            command.Add(0x24); // reg adr
            command.Add(0x02);


            byte[] commandToSend = appendChecksum(command);
            serialPort.DataReceived -= serialPort_DataReceived;
            serialPort.Write(commandToSend, 0, commandToSend.Length);
            System.Threading.Thread.Sleep(100);
            List<int> result = new List<int>();
            for (int i = 0; i < 8; i++)
                result.Add(serialPort.ReadByte());

            serialPort.DataReceived += serialPort_DataReceived;

            int lower = result[5];
            int upper = result[6];

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
            serialPort.Write(commandToSend, 0, commandToSend.Length);
        }
        internal void SetTrackeralgoOverride(bool overrideActive)
        {
            //ToDo
        }
    }
}
