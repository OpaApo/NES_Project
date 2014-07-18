namespace NES_Suntracker_Console
{
    partial class MainWindow
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.forceActuator_btn = new System.Windows.Forms.Button();
            this.temp_txtbx = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pos_txtbx = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.voltage_txtbx = new System.Windows.Forms.TextBox();
            this.quit_btn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.intens3_txtbx = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.intens2_txtbx = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.sunangle_txtbx = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.intens4_txtbx = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.intens1_txtbx = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.trackeralgoOverride_chkbx = new System.Windows.Forms.CheckBox();
            this.gotoposition_btn = new System.Windows.Forms.Button();
            this.gotoposition_txtbx = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.solarvoltage_txtbx = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.poller = new System.Windows.Forms.Timer(this.components);
            this.lastUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.forceActuator_btn);
            this.groupBox1.Controls.Add(this.temp_txtbx);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.pos_txtbx);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.voltage_txtbx);
            this.groupBox1.Location = new System.Drawing.Point(18, 90);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(444, 255);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actuator Status";
            // 
            // forceActuator_btn
            // 
            this.forceActuator_btn.Location = new System.Drawing.Point(93, 195);
            this.forceActuator_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.forceActuator_btn.Name = "forceActuator_btn";
            this.forceActuator_btn.Size = new System.Drawing.Size(268, 44);
            this.forceActuator_btn.TabIndex = 8;
            this.forceActuator_btn.Text = "Force Update";
            this.forceActuator_btn.UseVisualStyleBackColor = true;
            this.forceActuator_btn.Click += new System.EventHandler(this.forceActuator_btn_Click);
            // 
            // temp_txtbx
            // 
            this.temp_txtbx.Enabled = false;
            this.temp_txtbx.Location = new System.Drawing.Point(247, 39);
            this.temp_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.temp_txtbx.Name = "temp_txtbx";
            this.temp_txtbx.ReadOnly = true;
            this.temp_txtbx.Size = new System.Drawing.Size(148, 31);
            this.temp_txtbx.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 44);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(214, 25);
            this.label7.TabIndex = 12;
            this.label7.Text = "Present Temperature";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 91);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 25);
            this.label6.TabIndex = 10;
            this.label6.Text = "Present Voltage";
            // 
            // pos_txtbx
            // 
            this.pos_txtbx.Enabled = false;
            this.pos_txtbx.Location = new System.Drawing.Point(247, 127);
            this.pos_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pos_txtbx.Name = "pos_txtbx";
            this.pos_txtbx.ReadOnly = true;
            this.pos_txtbx.Size = new System.Drawing.Size(148, 31);
            this.pos_txtbx.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 134);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Present Position";
            // 
            // voltage_txtbx
            // 
            this.voltage_txtbx.Enabled = false;
            this.voltage_txtbx.Location = new System.Drawing.Point(247, 83);
            this.voltage_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.voltage_txtbx.Name = "voltage_txtbx";
            this.voltage_txtbx.ReadOnly = true;
            this.voltage_txtbx.Size = new System.Drawing.Size(148, 31);
            this.voltage_txtbx.TabIndex = 3;
            // 
            // quit_btn
            // 
            this.quit_btn.Location = new System.Drawing.Point(834, 759);
            this.quit_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.quit_btn.Name = "quit_btn";
            this.quit_btn.Size = new System.Drawing.Size(112, 44);
            this.quit_btn.TabIndex = 1;
            this.quit_btn.Text = "Quit";
            this.quit_btn.UseVisualStyleBackColor = true;
            this.quit_btn.Click += new System.EventHandler(this.quit_btn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.intens3_txtbx);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.intens2_txtbx);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.sunangle_txtbx);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.intens4_txtbx);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.intens1_txtbx);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Location = new System.Drawing.Point(502, 90);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(444, 353);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tracker Status";
            // 
            // intens3_txtbx
            // 
            this.intens3_txtbx.Enabled = false;
            this.intens3_txtbx.Location = new System.Drawing.Point(249, 153);
            this.intens3_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.intens3_txtbx.Name = "intens3_txtbx";
            this.intens3_txtbx.ReadOnly = true;
            this.intens3_txtbx.Size = new System.Drawing.Size(148, 31);
            this.intens3_txtbx.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 158);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(162, 25);
            this.label8.TabIndex = 12;
            this.label8.Text = "Intentity Node 3";
            // 
            // intens2_txtbx
            // 
            this.intens2_txtbx.Enabled = false;
            this.intens2_txtbx.Location = new System.Drawing.Point(249, 109);
            this.intens2_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.intens2_txtbx.Name = "intens2_txtbx";
            this.intens2_txtbx.ReadOnly = true;
            this.intens2_txtbx.Size = new System.Drawing.Size(148, 31);
            this.intens2_txtbx.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 205);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 25);
            this.label10.TabIndex = 10;
            this.label10.Text = "Intentity Node 4";
            // 
            // sunangle_txtbx
            // 
            this.sunangle_txtbx.Enabled = false;
            this.sunangle_txtbx.Location = new System.Drawing.Point(249, 241);
            this.sunangle_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sunangle_txtbx.Name = "sunangle_txtbx";
            this.sunangle_txtbx.ReadOnly = true;
            this.sunangle_txtbx.Size = new System.Drawing.Size(148, 31);
            this.sunangle_txtbx.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 248);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(226, 25);
            this.label12.TabIndex = 4;
            this.label12.Text = "Estimated Sunposition";
            // 
            // intens4_txtbx
            // 
            this.intens4_txtbx.Enabled = false;
            this.intens4_txtbx.Location = new System.Drawing.Point(249, 197);
            this.intens4_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.intens4_txtbx.Name = "intens4_txtbx";
            this.intens4_txtbx.ReadOnly = true;
            this.intens4_txtbx.Size = new System.Drawing.Size(148, 31);
            this.intens4_txtbx.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 114);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(162, 25);
            this.label13.TabIndex = 2;
            this.label13.Text = "Intentity Node 2";
            // 
            // intens1_txtbx
            // 
            this.intens1_txtbx.Enabled = false;
            this.intens1_txtbx.Location = new System.Drawing.Point(249, 66);
            this.intens1_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.intens1_txtbx.Name = "intens1_txtbx";
            this.intens1_txtbx.ReadOnly = true;
            this.intens1_txtbx.Size = new System.Drawing.Size(148, 31);
            this.intens1_txtbx.TabIndex = 1;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 70);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(162, 25);
            this.label14.TabIndex = 0;
            this.label14.Text = "Intentity Node 1";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(324, 23);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(353, 44);
            this.label15.TabIndex = 4;
            this.label15.Text = "Suntracker Console";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.trackeralgoOverride_chkbx);
            this.groupBox4.Controls.Add(this.gotoposition_btn);
            this.groupBox4.Controls.Add(this.gotoposition_txtbx);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Location = new System.Drawing.Point(502, 517);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(444, 213);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Manual Actuator Control";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(378, 94);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(25, 31);
            this.label20.TabIndex = 11;
            this.label20.Text = "°";
            // 
            // trackeralgoOverride_chkbx
            // 
            this.trackeralgoOverride_chkbx.AutoSize = true;
            this.trackeralgoOverride_chkbx.Location = new System.Drawing.Point(14, 50);
            this.trackeralgoOverride_chkbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackeralgoOverride_chkbx.Name = "trackeralgoOverride_chkbx";
            this.trackeralgoOverride_chkbx.Size = new System.Drawing.Size(301, 29);
            this.trackeralgoOverride_chkbx.TabIndex = 10;
            this.trackeralgoOverride_chkbx.Text = "Tracker Algorithm Override";
            this.trackeralgoOverride_chkbx.UseVisualStyleBackColor = true;
            this.trackeralgoOverride_chkbx.CheckedChanged += new System.EventHandler(this.trackeralgoOverride_chkbx_CheckedChanged);
            // 
            // gotoposition_btn
            // 
            this.gotoposition_btn.Enabled = false;
            this.gotoposition_btn.Location = new System.Drawing.Point(322, 138);
            this.gotoposition_btn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gotoposition_btn.Name = "gotoposition_btn";
            this.gotoposition_btn.Size = new System.Drawing.Size(112, 56);
            this.gotoposition_btn.TabIndex = 7;
            this.gotoposition_btn.Text = "Go";
            this.gotoposition_btn.UseVisualStyleBackColor = true;
            this.gotoposition_btn.Click += new System.EventHandler(this.gotoposition_btn_Click);
            // 
            // gotoposition_txtbx
            // 
            this.gotoposition_txtbx.Enabled = false;
            this.gotoposition_txtbx.Location = new System.Drawing.Point(303, 94);
            this.gotoposition_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gotoposition_txtbx.Name = "gotoposition_txtbx";
            this.gotoposition_txtbx.Size = new System.Drawing.Size(73, 31);
            this.gotoposition_txtbx.TabIndex = 9;
            this.gotoposition_txtbx.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(9, 102);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(157, 25);
            this.label17.TabIndex = 8;
            this.label17.Text = "Target Position";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.solarvoltage_txtbx);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(18, 400);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(444, 124);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Solarpanel";
            // 
            // solarvoltage_txtbx
            // 
            this.solarvoltage_txtbx.Enabled = false;
            this.solarvoltage_txtbx.Location = new System.Drawing.Point(240, 51);
            this.solarvoltage_txtbx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.solarvoltage_txtbx.Name = "solarvoltage_txtbx";
            this.solarvoltage_txtbx.ReadOnly = true;
            this.solarvoltage_txtbx.Size = new System.Drawing.Size(148, 31);
            this.solarvoltage_txtbx.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(200, 25);
            this.label9.TabIndex = 0;
            this.label9.Text = "Measured intensity:";
            // 
            // poller
            // 
            this.poller.Enabled = true;
            this.poller.Interval = 5000;
            this.poller.Tick += new System.EventHandler(this.poller_Tick);
            // 
            // lastUpdateTimer
            // 
            this.lastUpdateTimer.Enabled = true;
            this.lastUpdateTimer.Interval = 500;
            this.lastUpdateTimer.Tick += new System.EventHandler(this.lastUpdateTimer_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 848);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.quit_btn);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainWindow";
            this.Text = "Suntracker Console";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox temp_txtbx;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox pos_txtbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox voltage_txtbx;
        private System.Windows.Forms.Button quit_btn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox intens3_txtbx;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox intens2_txtbx;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox sunangle_txtbx;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox intens4_txtbx;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox intens1_txtbx;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox trackeralgoOverride_chkbx;
        private System.Windows.Forms.Button gotoposition_btn;
        private System.Windows.Forms.TextBox gotoposition_txtbx;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox solarvoltage_txtbx;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer poller;
        private System.Windows.Forms.Button forceActuator_btn;
        private System.Windows.Forms.Timer lastUpdateTimer;
    }
}

