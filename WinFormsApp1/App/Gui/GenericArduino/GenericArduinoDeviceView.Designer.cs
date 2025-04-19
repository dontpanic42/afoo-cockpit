namespace AFooCockpit.App.Gui
{
    partial class GenericArduinoDeviceView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            toolStrip1 = new ToolStrip();
            groupBox1 = new GroupBox();
            toolStrip2 = new ToolStrip();
            tsbAddDataSource = new ToolStripButton();
            tsbEditDataSource = new ToolStripButton();
            tsbDeleteDataSource = new ToolStripButton();
            lsbDataSources = new ListBox();
            groupBox2 = new GroupBox();
            toolStrip3 = new ToolStrip();
            tsbAddDevice = new ToolStripButton();
            tsbRemoveDevice = new ToolStripButton();
            lsbDevices = new ListBox();
            groupBox1.SuspendLayout();
            toolStrip2.SuspendLayout();
            groupBox2.SuspendLayout();
            toolStrip3.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1084, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupBox1.Controls.Add(toolStrip2);
            groupBox1.Controls.Add(lsbDataSources);
            groupBox1.Location = new Point(3, 28);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(517, 540);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Arduinos";
            // 
            // toolStrip2
            // 
            toolStrip2.Dock = DockStyle.Right;
            toolStrip2.Items.AddRange(new ToolStripItem[] { tsbAddDataSource, tsbEditDataSource, tsbDeleteDataSource });
            toolStrip2.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            toolStrip2.Location = new Point(490, 19);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new Size(24, 518);
            toolStrip2.TabIndex = 4;
            toolStrip2.Text = "toolStrip2";
            // 
            // tsbAddDataSource
            // 
            tsbAddDataSource.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddDataSource.Image = Properties.Resources.add;
            tsbAddDataSource.ImageTransparentColor = Color.Magenta;
            tsbAddDataSource.Name = "tsbAddDataSource";
            tsbAddDataSource.Size = new Size(21, 20);
            tsbAddDataSource.Text = "Add Data Source";
            tsbAddDataSource.Click += tsbAddDataSource_Click;
            // 
            // tsbEditDataSource
            // 
            tsbEditDataSource.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbEditDataSource.Image = Properties.Resources.pencil;
            tsbEditDataSource.ImageTransparentColor = Color.Magenta;
            tsbEditDataSource.Name = "tsbEditDataSource";
            tsbEditDataSource.Size = new Size(21, 20);
            tsbEditDataSource.Text = "Edit Data Source";
            tsbEditDataSource.Click += tsbEditDataSource_Click;
            // 
            // tsbDeleteDataSource
            // 
            tsbDeleteDataSource.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDeleteDataSource.Image = Properties.Resources.remove;
            tsbDeleteDataSource.ImageTransparentColor = Color.Magenta;
            tsbDeleteDataSource.Name = "tsbDeleteDataSource";
            tsbDeleteDataSource.Size = new Size(21, 20);
            tsbDeleteDataSource.Text = "Delete Data Source";
            tsbDeleteDataSource.Click += tsbDeleteDataSource_Click;
            // 
            // lsbDataSources
            // 
            lsbDataSources.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lsbDataSources.FormattingEnabled = true;
            lsbDataSources.ItemHeight = 15;
            lsbDataSources.Location = new Point(3, 19);
            lsbDataSources.Name = "lsbDataSources";
            lsbDataSources.Size = new Size(479, 514);
            lsbDataSources.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(toolStrip3);
            groupBox2.Controls.Add(lsbDevices);
            groupBox2.Location = new Point(526, 28);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(555, 540);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Devices";
            // 
            // toolStrip3
            // 
            toolStrip3.Dock = DockStyle.Right;
            toolStrip3.Items.AddRange(new ToolStripItem[] { tsbAddDevice, tsbRemoveDevice });
            toolStrip3.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            toolStrip3.Location = new Point(528, 19);
            toolStrip3.Name = "toolStrip3";
            toolStrip3.Size = new Size(24, 518);
            toolStrip3.TabIndex = 5;
            toolStrip3.Text = "toolStrip3";
            // 
            // tsbAddDevice
            // 
            tsbAddDevice.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbAddDevice.Image = Properties.Resources.add;
            tsbAddDevice.ImageTransparentColor = Color.Magenta;
            tsbAddDevice.Name = "tsbAddDevice";
            tsbAddDevice.Size = new Size(21, 20);
            tsbAddDevice.Text = "toolStripButton1";
            tsbAddDevice.Click += tsbAddDevice_Click;
            // 
            // tsbRemoveDevice
            // 
            tsbRemoveDevice.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbRemoveDevice.Image = Properties.Resources.remove;
            tsbRemoveDevice.ImageTransparentColor = Color.Magenta;
            tsbRemoveDevice.Name = "tsbRemoveDevice";
            tsbRemoveDevice.Size = new Size(21, 20);
            tsbRemoveDevice.Text = "toolStripButton1";
            tsbRemoveDevice.Click += tsbRemoveDevice_Click;
            // 
            // lsbDevices
            // 
            lsbDevices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lsbDevices.FormattingEnabled = true;
            lsbDevices.ItemHeight = 15;
            lsbDevices.Location = new Point(6, 19);
            lsbDevices.Name = "lsbDevices";
            lsbDevices.Size = new Size(517, 514);
            lsbDevices.TabIndex = 0;
            // 
            // GenericArduinoDeviceView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(toolStrip1);
            Name = "GenericArduinoDeviceView";
            Size = new Size(1084, 571);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            toolStrip3.ResumeLayout(false);
            toolStrip3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private GroupBox groupBox1;
        private Button btnAddDataSource;
        private ListBox lsbDataSources;
        private GroupBox groupBox2;
        private ListBox lsbDevices;
        private Button btnEditDataSource;
        private ToolStrip toolStrip2;
        private ToolStripButton tsbAddDataSource;
        private ToolStripButton tsbEditDataSource;
        private ToolStripButton tsbDeleteDataSource;
        private ToolStrip toolStrip3;
        private ToolStripButton tsbAddDevice;
        private ToolStripButton tsbRemoveDevice;
    }
}
