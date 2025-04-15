namespace WinFormsApp1
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            toolStripSeparator1 = new ToolStripSeparator();
            tsbConnect = new ToolStripButton();
            tsbDisconnect = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            tsbEventView = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            tsbSaveSettings = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            tssStatus = new ToolStripStatusLabel();
            tssAircraft = new ToolStripStatusLabel();
            tabControl1 = new TabControl();
            tabCCUsbDevices = new TabPage();
            dgvArinc429Devices = new AFooCockpit.App.Gui.Arinc429TranscieverGridView();
            tabArduinoDevices = new TabPage();
            dgvArduinoDevices = new AFooCockpit.App.Gui.ArduinoDeviceGridView();
            tabSerialDevices = new TabPage();
            dgvSerialDevices = new AFooCockpit.App.Gui.SerialDeviceGridView();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabCCUsbDevices.SuspendLayout();
            tabArduinoDevices.SuspendLayout();
            tabSerialDevices.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, toolStripSeparator1, tsbConnect, tsbDisconnect, toolStripSeparator2, tsbEventView, toolStripSeparator3, tsbSaveSettings });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1342, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(76, 22);
            toolStripLabel1.Text = "AFooCockpit";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // tsbConnect
            // 
            tsbConnect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbConnect.Image = AFooCockpit.Properties.Resources.broken_link;
            tsbConnect.ImageTransparentColor = Color.Magenta;
            tsbConnect.Name = "tsbConnect";
            tsbConnect.Size = new Size(23, 22);
            tsbConnect.Text = "Connect";
            tsbConnect.Click += tsbConnect_Click;
            // 
            // tsbDisconnect
            // 
            tsbDisconnect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDisconnect.Enabled = false;
            tsbDisconnect.Image = AFooCockpit.Properties.Resources.disconnect;
            tsbDisconnect.ImageTransparentColor = Color.Magenta;
            tsbDisconnect.Name = "tsbDisconnect";
            tsbDisconnect.Size = new Size(23, 22);
            tsbDisconnect.Text = "toolStripButton1";
            tsbDisconnect.ToolTipText = "Disconnect";
            tsbDisconnect.Click += tsbDisconnect_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // tsbEventView
            // 
            tsbEventView.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbEventView.Enabled = false;
            tsbEventView.Image = AFooCockpit.Properties.Resources.lightning_bolt;
            tsbEventView.ImageTransparentColor = Color.Magenta;
            tsbEventView.Name = "tsbEventView";
            tsbEventView.Size = new Size(23, 22);
            tsbEventView.Text = "Show Events";
            tsbEventView.Click += tspEventView_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 25);
            // 
            // tsbSaveSettings
            // 
            tsbSaveSettings.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbSaveSettings.Image = AFooCockpit.Properties.Resources.save_file;
            tsbSaveSettings.ImageTransparentColor = Color.Magenta;
            tsbSaveSettings.Name = "tsbSaveSettings";
            tsbSaveSettings.Size = new Size(23, 22);
            tsbSaveSettings.Text = "toolStripButton1";
            tsbSaveSettings.ToolTipText = "Save Settings";
            tsbSaveSettings.Click += tsbSaveSettings_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { tssStatus, tssAircraft });
            statusStrip1.Location = new Point(0, 672);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1342, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // tssStatus
            // 
            tssStatus.Name = "tssStatus";
            tssStatus.Size = new Size(26, 17);
            tssStatus.Text = "Idle";
            // 
            // tssAircraft
            // 
            tssAircraft.Name = "tssAircraft";
            tssAircraft.Size = new Size(71, 17);
            tssAircraft.Text = "(no Aircraft)";
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabCCUsbDevices);
            tabControl1.Controls.Add(tabArduinoDevices);
            tabControl1.Controls.Add(tabSerialDevices);
            tabControl1.Location = new Point(0, 28);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1342, 641);
            tabControl1.TabIndex = 5;
            // 
            // tabCCUsbDevices
            // 
            tabCCUsbDevices.Controls.Add(dgvArinc429Devices);
            tabCCUsbDevices.Location = new Point(4, 24);
            tabCCUsbDevices.Name = "tabCCUsbDevices";
            tabCCUsbDevices.Size = new Size(1334, 613);
            tabCCUsbDevices.TabIndex = 2;
            tabCCUsbDevices.Text = "Arinc429 Devices";
            tabCCUsbDevices.UseVisualStyleBackColor = true;
            // 
            // dgvArinc429Devices
            // 
            dgvArinc429Devices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvArinc429Devices.Location = new Point(0, 0);
            dgvArinc429Devices.Margin = new Padding(0);
            dgvArinc429Devices.Name = "dgvArinc429Devices";
            dgvArinc429Devices.Size = new Size(1334, 613);
            dgvArinc429Devices.TabIndex = 0;
            // 
            // tabArduinoDevices
            // 
            tabArduinoDevices.Controls.Add(dgvArduinoDevices);
            tabArduinoDevices.Location = new Point(4, 24);
            tabArduinoDevices.Name = "tabArduinoDevices";
            tabArduinoDevices.Padding = new Padding(3);
            tabArduinoDevices.Size = new Size(1334, 613);
            tabArduinoDevices.TabIndex = 1;
            tabArduinoDevices.Text = "OEM";
            tabArduinoDevices.UseVisualStyleBackColor = true;
            // 
            // dgvArduinoDevices
            // 
            dgvArduinoDevices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvArduinoDevices.Location = new Point(0, 0);
            dgvArduinoDevices.Name = "dgvArduinoDevices";
            dgvArduinoDevices.Size = new Size(1334, 613);
            dgvArduinoDevices.TabIndex = 0;
            // 
            // tabSerialDevices
            // 
            tabSerialDevices.Controls.Add(dgvSerialDevices);
            tabSerialDevices.Location = new Point(4, 24);
            tabSerialDevices.Name = "tabSerialDevices";
            tabSerialDevices.Padding = new Padding(3);
            tabSerialDevices.Size = new Size(1334, 613);
            tabSerialDevices.TabIndex = 0;
            tabSerialDevices.Text = "Serial Devices";
            tabSerialDevices.UseVisualStyleBackColor = true;
            // 
            // dgvSerialDevices
            // 
            dgvSerialDevices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvSerialDevices.Location = new Point(0, 0);
            dgvSerialDevices.Name = "dgvSerialDevices";
            dgvSerialDevices.Size = new Size(1334, 613);
            dgvSerialDevices.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1342, 694);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Name = "MainForm";
            Text = "Form1";
            FormClosing += MainForm_FormClosing;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabCCUsbDevices.ResumeLayout(false);
            tabArduinoDevices.ResumeLayout(false);
            tabSerialDevices.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsbConnect;
        private ToolStripButton tsbEventView;
        private ToolStripSeparator toolStripSeparator2;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tssStatus;
        private ToolStripStatusLabel tssAircraft;
        private TabControl tabControl1;
        private TabPage tabSerialDevices;
        private AFooCockpit.App.Gui.SerialDeviceGridView dgvSerialDevices;
        private ToolStripButton tsbDisconnect;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton tsbSaveSettings;
        private TabPage tabArduinoDevices;
        private AFooCockpit.App.Gui.ArduinoDeviceGridView dgvArduinoDevices;
        private TabPage tabCCUsbDevices;
        private AFooCockpit.App.Gui.Arinc429TranscieverGridView dgvArinc429Devices;
    }
}
