namespace WinFormsApp1
{
    partial class Form1
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
            dgvDevices = new AFooCockpit.App.Gui.DeviceGridView();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            toolStripSeparator1 = new ToolStripSeparator();
            tsbConnect = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            tsbEventView = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            tssStatus = new ToolStripStatusLabel();
            tssAircraft = new ToolStripStatusLabel();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvDevices
            // 
            dgvDevices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDevices.Location = new Point(12, 28);
            dgvDevices.Name = "dgvDevices";
            dgvDevices.Size = new Size(776, 419);
            dgvDevices.TabIndex = 2;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, toolStripSeparator1, tsbConnect, toolStripSeparator2, tsbEventView });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
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
            tsbConnect.Text = "toolStripButton1";
            tsbConnect.Click += tsbConnect_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 25);
            // 
            // tsbEventView
            // 
            tsbEventView.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbEventView.Image = AFooCockpit.Properties.Resources.lightning_bolt;
            tsbEventView.ImageTransparentColor = Color.Magenta;
            tsbEventView.Name = "tsbEventView";
            tsbEventView.Size = new Size(23, 22);
            tsbEventView.Text = "toolStripButton1";
            tsbEventView.Click += tspEventView_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { tssStatus, tssAircraft });
            statusStrip1.Location = new Point(0, 437);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 459);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Controls.Add(dgvDevices);
            Name = "Form1";
            Text = "Form1";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private AFooCockpit.App.Gui.DeviceGridView dgvDevices;
        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsbConnect;
        private ToolStripButton tsbEventView;
        private ToolStripSeparator toolStripSeparator2;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tssStatus;
        private ToolStripStatusLabel tssAircraft;
    }
}
