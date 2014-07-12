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
            Communicator.GetInstance();
            System.Threading.Thread.Sleep(300); // wait for init package to complete
            temp_txtbx.Text = Communicator.GetInstance().getTemperture() + "°C";
            voltage_txtbx.Text = Communicator.GetInstance().getVoltage() + "V";
            pos_txtbx.Text = Communicator.GetInstance().getPosition().ToString() + "°";
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
            if(Int16.Parse(gotoposition_txtbx.Text) < 0 || Int16.Parse(gotoposition_txtbx.Text) >300)
            {
                MessageBox.Show("Valid: 0°<=angle<=300°", "Invalid angle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Communicator.GetInstance().SendActuatorPosition(Int16.Parse(gotoposition_txtbx.Text));
        }

        private void quit_btn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


        private void poller_Tick(object sender, EventArgs e)
        {
            temp_txtbx.Text = Communicator.GetInstance().getTemperture() + "°C";
            voltage_txtbx.Text = Communicator.GetInstance().getVoltage() + "V";
            pos_txtbx.Text = Communicator.GetInstance().getPosition().ToString() + "°";
           
        }

        private void forceActuator_btn_Click(object sender, EventArgs e)
        {
            poller.Enabled = false;
            temp_txtbx.Text = Communicator.GetInstance().getTemperture() + "°C";
            voltage_txtbx.Text = Communicator.GetInstance().getVoltage() + "V";
            pos_txtbx.Text = Communicator.GetInstance().getPosition().ToString() + "°";
            poller.Enabled = true;
        }
    }
}
