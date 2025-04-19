namespace AFooCockpit.App.Gui.DataSourceConfigForms
{
    partial class GenericArduinoDataSourceConfigView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOk = new Button();
            btnCancel = new Button();
            tabControl1 = new TabControl();
            tabPortConfiguration = new TabPage();
            btnRefreshComPorts = new Button();
            numBaudRate = new NumericUpDown();
            cmbSerialPort = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            tabPinConfiguration = new TabPage();
            selPinD13 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD12 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD11 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD10 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD9 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD8 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD7 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD6 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD5 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD4 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD3 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD2 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD1 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            selPinD0 = new GenericArduinoDataSourceConfigDigitalPinSelector();
            pictureBox1 = new PictureBox();
            tabControl1.SuspendLayout();
            tabPortConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numBaudRate).BeginInit();
            tabPinConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Location = new Point(964, 640);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 13;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(883, 640);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPortConfiguration);
            tabControl1.Controls.Add(tabPinConfiguration);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new Point(0, 0);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1031, 622);
            tabControl1.TabIndex = 15;
            // 
            // tabPortConfiguration
            // 
            tabPortConfiguration.Controls.Add(btnRefreshComPorts);
            tabPortConfiguration.Controls.Add(numBaudRate);
            tabPortConfiguration.Controls.Add(cmbSerialPort);
            tabPortConfiguration.Controls.Add(label2);
            tabPortConfiguration.Controls.Add(label1);
            tabPortConfiguration.Location = new Point(4, 24);
            tabPortConfiguration.Name = "tabPortConfiguration";
            tabPortConfiguration.Padding = new Padding(3);
            tabPortConfiguration.Size = new Size(1000, 488);
            tabPortConfiguration.TabIndex = 0;
            tabPortConfiguration.Text = "Connectivity";
            tabPortConfiguration.UseVisualStyleBackColor = true;
            // 
            // btnRefreshComPorts
            // 
            btnRefreshComPorts.Location = new Point(236, 12);
            btnRefreshComPorts.Name = "btnRefreshComPorts";
            btnRefreshComPorts.Size = new Size(75, 23);
            btnRefreshComPorts.TabIndex = 4;
            btnRefreshComPorts.Text = "Refresh";
            btnRefreshComPorts.UseVisualStyleBackColor = true;
            btnRefreshComPorts.Click += btnRefreshComPorts_Click;
            // 
            // numBaudRate
            // 
            numBaudRate.Location = new Point(100, 49);
            numBaudRate.Maximum = new decimal(new int[] { 460800, 0, 0, 0 });
            numBaudRate.Minimum = new decimal(new int[] { 9600, 0, 0, 0 });
            numBaudRate.Name = "numBaudRate";
            numBaudRate.Size = new Size(120, 23);
            numBaudRate.TabIndex = 3;
            numBaudRate.Value = new decimal(new int[] { 9600, 0, 0, 0 });
            // 
            // cmbSerialPort
            // 
            cmbSerialPort.FormattingEnabled = true;
            cmbSerialPort.Location = new Point(100, 12);
            cmbSerialPort.Name = "cmbSerialPort";
            cmbSerialPort.Size = new Size(121, 23);
            cmbSerialPort.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 51);
            label2.Name = "label2";
            label2.Size = new Size(60, 15);
            label2.TabIndex = 1;
            label2.Text = "Baud Rate";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 15);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 0;
            label1.Text = "Serial Port";
            // 
            // tabPinConfiguration
            // 
            tabPinConfiguration.Controls.Add(selPinD13);
            tabPinConfiguration.Controls.Add(selPinD12);
            tabPinConfiguration.Controls.Add(selPinD11);
            tabPinConfiguration.Controls.Add(selPinD10);
            tabPinConfiguration.Controls.Add(selPinD9);
            tabPinConfiguration.Controls.Add(selPinD8);
            tabPinConfiguration.Controls.Add(selPinD7);
            tabPinConfiguration.Controls.Add(selPinD6);
            tabPinConfiguration.Controls.Add(selPinD5);
            tabPinConfiguration.Controls.Add(selPinD4);
            tabPinConfiguration.Controls.Add(selPinD3);
            tabPinConfiguration.Controls.Add(selPinD2);
            tabPinConfiguration.Controls.Add(selPinD1);
            tabPinConfiguration.Controls.Add(selPinD0);
            tabPinConfiguration.Controls.Add(pictureBox1);
            tabPinConfiguration.Location = new Point(4, 24);
            tabPinConfiguration.Name = "tabPinConfiguration";
            tabPinConfiguration.Padding = new Padding(3);
            tabPinConfiguration.Size = new Size(1023, 594);
            tabPinConfiguration.TabIndex = 1;
            tabPinConfiguration.Text = "IO Configuration";
            tabPinConfiguration.UseVisualStyleBackColor = true;
            // 
            // selPinD13
            // 
            selPinD13.Location = new Point(741, 506);
            selPinD13.Name = "selPinD13";
            selPinD13.PinName = "Digital 13";
            selPinD13.Size = new Size(276, 82);
            selPinD13.TabIndex = 14;
            // 
            // selPinD12
            // 
            selPinD12.Location = new Point(741, 421);
            selPinD12.Name = "selPinD12";
            selPinD12.PinName = "Digital 12";
            selPinD12.Size = new Size(276, 82);
            selPinD12.TabIndex = 13;
            // 
            // selPinD11
            // 
            selPinD11.Location = new Point(741, 333);
            selPinD11.Name = "selPinD11";
            selPinD11.PinName = "Digital 11";
            selPinD11.Size = new Size(276, 82);
            selPinD11.TabIndex = 12;
            // 
            // selPinD10
            // 
            selPinD10.Location = new Point(741, 245);
            selPinD10.Name = "selPinD10";
            selPinD10.PinName = "Digital 10";
            selPinD10.Size = new Size(276, 82);
            selPinD10.TabIndex = 11;
            // 
            // selPinD9
            // 
            selPinD9.Location = new Point(741, 166);
            selPinD9.Name = "selPinD9";
            selPinD9.PinName = "Digital 9";
            selPinD9.Size = new Size(276, 82);
            selPinD9.TabIndex = 10;
            // 
            // selPinD8
            // 
            selPinD8.Location = new Point(741, 86);
            selPinD8.Name = "selPinD8";
            selPinD8.PinName = "Digital 8";
            selPinD8.Size = new Size(276, 82);
            selPinD8.TabIndex = 9;
            // 
            // selPinD7
            // 
            selPinD7.Location = new Point(741, 6);
            selPinD7.Name = "selPinD7";
            selPinD7.PinName = "Digital 7";
            selPinD7.Size = new Size(276, 82);
            selPinD7.TabIndex = 8;
            // 
            // selPinD6
            // 
            selPinD6.Location = new Point(6, 506);
            selPinD6.Name = "selPinD6";
            selPinD6.PinName = "Digital 6";
            selPinD6.Size = new Size(276, 82);
            selPinD6.TabIndex = 7;
            // 
            // selPinD5
            // 
            selPinD5.Location = new Point(6, 421);
            selPinD5.Name = "selPinD5";
            selPinD5.PinName = "Digital 5";
            selPinD5.Size = new Size(276, 82);
            selPinD5.TabIndex = 6;
            // 
            // selPinD4
            // 
            selPinD4.Location = new Point(6, 333);
            selPinD4.Name = "selPinD4";
            selPinD4.PinName = "Digital 4";
            selPinD4.Size = new Size(276, 82);
            selPinD4.TabIndex = 5;
            // 
            // selPinD3
            // 
            selPinD3.Location = new Point(6, 245);
            selPinD3.Name = "selPinD3";
            selPinD3.PinName = "Digital 3";
            selPinD3.Size = new Size(276, 82);
            selPinD3.TabIndex = 4;
            // 
            // selPinD2
            // 
            selPinD2.Location = new Point(6, 166);
            selPinD2.Name = "selPinD2";
            selPinD2.PinName = "Digital 2";
            selPinD2.Size = new Size(276, 82);
            selPinD2.TabIndex = 3;
            // 
            // selPinD1
            // 
            selPinD1.Location = new Point(6, 86);
            selPinD1.Name = "selPinD1";
            selPinD1.PinName = "Digital 1";
            selPinD1.Size = new Size(276, 82);
            selPinD1.TabIndex = 2;
            // 
            // selPinD0
            // 
            selPinD0.Location = new Point(6, 6);
            selPinD0.Name = "selPinD0";
            selPinD0.PinName = "Digital 0";
            selPinD0.Size = new Size(276, 82);
            selPinD0.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Arduino_Nano_Pin_Layout_In_this_system_we_have_use_the_Arduino_Nano_30_which_is_a_30_pin;
            pictureBox1.Location = new Point(308, 47);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(427, 487);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // GenericArduinoDataSourceConfigView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1053, 673);
            Controls.Add(tabControl1);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "GenericArduinoDataSourceConfigView";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Generic Arduino Configuration";
            tabControl1.ResumeLayout(false);
            tabPortConfiguration.ResumeLayout(false);
            tabPortConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numBaudRate).EndInit();
            tabPinConfiguration.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button btnOk;
        private Button btnCancel;
        private TabControl tabControl1;
        private TabPage tabPortConfiguration;
        private TabPage tabPinConfiguration;
        private NumericUpDown numBaudRate;
        private ComboBox cmbSerialPort;
        private Label label2;
        private Label label1;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD8;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD7;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD6;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD5;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD4;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD3;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD2;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD1;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD0;
        private PictureBox pictureBox1;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD13;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD12;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD11;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD10;
        private GenericArduinoDataSourceConfigDigitalPinSelector selPinD9;
        private Button btnRefreshComPorts;
    }
}