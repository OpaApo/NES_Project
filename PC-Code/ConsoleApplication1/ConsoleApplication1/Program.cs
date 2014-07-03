using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static SerialPort serialPort;
        static void Main(string[] args)
        {
             serialPort = new SerialPort();
            string VER_Command = "VER\r";

            if (serialPort is SerialPort)
            {
                serialPort.PortName = "COM4";
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.BaudRate = 115200;

                try
                {
                    serialPort.Open();
                    serialPort.DiscardOutBuffer();
                    serialPort.DiscardInBuffer();

                    serialPort.DataReceived += new SerialDataReceivedEventHandler(responseHandler);
                    serialPort.Write(VER_Command);
                }
                catch (Exception exc)
                {
                }// end CATCH portion of TRY/CATCH block
            }// end IF serialPort is viable
            Console.ReadLine();

        }

        private static void responseHandler(object sender, SerialDataReceivedEventArgs args)
        {
            System.Threading.Thread.Sleep(100);
            string x = serialPort.ReadExisting();
            char[] values = x.ToCharArray();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                string hexOutput = String.Format("{00:X}", value);
                Console.Write("0x{0} ", hexOutput);
            }
            Console.WriteLine("\n");
        }
    }
}
