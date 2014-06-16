using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NES_Suntracker_Console
{
    class Communicator
    {
        private static Communicator instance = null;
        private Communicator()
        {
            //Open com etc
        }
        internal static Communicator GetInstance()
        {
            if (instance == null)
                instance = new Communicator();
            return instance;
        }
        internal void SendActuatorPosition(int position)
        {
            //ToDo
        }
        internal void SetTrackeralgoOverride(bool overrideActive)
        {
            //ToDo
        }
    }
}
