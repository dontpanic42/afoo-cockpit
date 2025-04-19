namespace AFooCockpit.App.Gui.DeviceConfigForms
{
    partial class GenericArduinoDeviceConfigView
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
            groupBox1 = new GroupBox();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label1 = new Label();
            cmbDataSources = new ComboBox();
            cmbDevices = new ComboBox();
            btnOk = new Button();
            btnCancel = new Button();
            label3 = new Label();
            txtDisplayName = new TextBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtDisplayName);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(cmbDataSources);
            groupBox1.Controls.Add(cmbDevices);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(368, 175);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Device Selection";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.broken_link;
            pictureBox1.Location = new Point(173, 56);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(44, 23);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 28);
            label2.Name = "label2";
            label2.Size = new Size(73, 15);
            label2.TabIndex = 3;
            label2.Text = "Data Source:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(242, 28);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 2;
            label1.Text = "Device:";
            // 
            // cmbDataSources
            // 
            cmbDataSources.FormattingEnabled = true;
            cmbDataSources.Location = new Point(23, 56);
            cmbDataSources.Name = "cmbDataSources";
            cmbDataSources.Size = new Size(121, 23);
            cmbDataSources.TabIndex = 1;
            // 
            // cmbDevices
            // 
            cmbDevices.FormattingEnabled = true;
            cmbDevices.Location = new Point(241, 56);
            cmbDevices.Name = "cmbDevices";
            cmbDevices.Size = new Size(121, 23);
            cmbDevices.TabIndex = 0;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(305, 193);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 1;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(224, 193);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 96);
            label3.Name = "label3";
            label3.Size = new Size(121, 15);
            label3.TabIndex = 5;
            label3.Text = "Unique Device Name:";
            // 
            // txtDisplayName
            // 
            txtDisplayName.Location = new Point(23, 129);
            txtDisplayName.Name = "txtDisplayName";
            txtDisplayName.Size = new Size(339, 23);
            txtDisplayName.TabIndex = 6;
            // 
            // GenericArduinoDeviceConfigView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(392, 225);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(groupBox1);
            Name = "GenericArduinoDeviceConfigView";
            Text = "Create Device";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private ComboBox cmbDataSources;
        private ComboBox cmbDevices;
        private Button btnOk;
        private Button btnCancel;
        private PictureBox pictureBox1;
        private TextBox txtDisplayName;
        private Label label3;
    }
}