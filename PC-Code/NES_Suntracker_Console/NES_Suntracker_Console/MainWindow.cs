using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NES_Suntracker_Console
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void trackeralgoOverride_chkbx_CheckedChanged(object sender, EventArgs e)
        {
            if(trackeralgoOverride_chkbx.Checked)
            {
                gotoposition_btn.Enabled = true;
                gotoposition_txtbx.Enabled = true;
                Communicator.GetInstance().SetTrackeralgoOverride(true);
                return;
            }
            gotoposition_btn.Enabled = false;
            gotoposition_txtbx.Enabled = false;
            Communicator.GetInstance().SetTrackeralgoOverride(false);
        }

        private void gotoposition_btn_Click(object sender, EventArgs e)
        {
            Communicator.GetInstance().SendActuatorPosition(Utils.AngleToActPos(double.Parse(gotoposition_txtbx.Text)));
        }

        private void quit_btn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void manualActuator_chkbx_CheckedChanged(object sender, EventArgs e)
        {
            if(manualActuator_chkbx.Checked)
            {
                command_txtbx.Enabled = true;
                sendCommand_btn.Enabled = true;
            }
            else
            {
                command_txtbx.Enabled = false;
                sendCommand_btn.Enabled = false;
            }
        }

    }
}
