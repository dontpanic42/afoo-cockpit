namespace AFooCockpit.App.Gui.DataSourceConfigForms
{
    partial class GenericArduinoDataSourceConfigDigitalPinSelector
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
            chkPinEnabled = new CheckBox();
            cmbInputOutput = new ComboBox();
            cmbPullUp = new ComboBox();
            label1 = new Label();
            txtPinId = new TextBox();
            SuspendLayout();
            // 
            // chkPinEnabled
            // 
            chkPinEnabled.AutoSize = true;
            chkPinEnabled.Location = new Point(15, 15);
            chkPinEnabled.Name = "chkPinEnabled";
            chkPinEnabled.Size = new Size(78, 19);
            chkPinEnabled.TabIndex = 0;
            chkPinEnabled.Text = "Pin Name";
            chkPinEnabled.UseVisualStyleBackColor = true;
            chkPinEnabled.CheckedChanged += chkPinEnabled_CheckedChanged;
            // 
            // cmbInputOutput
            // 
            cmbInputOutput.FormattingEnabled = true;
            cmbInputOutput.Items.AddRange(new object[] { "Input", "Output" });
            cmbInputOutput.Location = new Point(99, 13);
            cmbInputOutput.Name = "cmbInputOutput";
            cmbInputOutput.Size = new Size(80, 23);
            cmbInputOutput.TabIndex = 1;
            // 
            // cmbPullUp
            // 
            cmbPullUp.FormattingEnabled = true;
            cmbPullUp.Items.AddRange(new object[] { "Pullup", "No Pullup" });
            cmbPullUp.Location = new Point(185, 13);
            cmbPullUp.Name = "cmbPullUp";
            cmbPullUp.Size = new Size(79, 23);
            cmbPullUp.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(33, 46);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 3;
            label1.Text = "Pin ID";
            // 
            // txtPinId
            // 
            txtPinId.Location = new Point(99, 43);
            txtPinId.Name = "txtPinId";
            txtPinId.Size = new Size(165, 23);
            txtPinId.TabIndex = 4;
            // 
            // GenericArduinoDataSourceConfigDigitalPinSelector
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtPinId);
            Controls.Add(label1);
            Controls.Add(cmbPullUp);
            Controls.Add(cmbInputOutput);
            Controls.Add(chkPinEnabled);
            Name = "GenericArduinoDataSourceConfigDigitalPinSelector";
            Size = new Size(276, 79);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chkPinEnabled;
        private ComboBox cmbInputOutput;
        private ComboBox cmbPullUp;
        private Label label1;
        private TextBox txtPinId;
    }
}
