namespace AFooCockpit.App.Gui
{
    partial class DebugView
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
            btnSend = new Button();
            txtEventData = new NumericUpDown();
            cobEventName = new ComboBox();
            lvLog = new ListBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtEventData).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnSend);
            groupBox1.Controls.Add(txtEventData);
            groupBox1.Controls.Add(cobEventName);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(410, 57);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Simulate Event";
            // 
            // btnSend
            // 
            btnSend.Location = new Point(324, 22);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(75, 23);
            btnSend.TabIndex = 2;
            btnSend.Text = "Send Event";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtEventData
            // 
            txtEventData.DecimalPlaces = 2;
            txtEventData.Location = new Point(213, 22);
            txtEventData.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            txtEventData.Name = "txtEventData";
            txtEventData.Size = new Size(105, 23);
            txtEventData.TabIndex = 1;
            // 
            // cobEventName
            // 
            cobEventName.FormattingEnabled = true;
            cobEventName.Location = new Point(6, 22);
            cobEventName.Name = "cobEventName";
            cobEventName.Size = new Size(201, 23);
            cobEventName.TabIndex = 0;
            // 
            // lvLog
            // 
            lvLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvLog.FormattingEnabled = true;
            lvLog.ItemHeight = 15;
            lvLog.Location = new Point(12, 75);
            lvLog.Name = "lvLog";
            lvLog.Size = new Size(731, 499);
            lvLog.TabIndex = 1;
            // 
            // DebugView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(753, 580);
            Controls.Add(lvLog);
            Controls.Add(groupBox1);
            Name = "DebugView";
            Text = "Bus Debug";
            FormClosing += DebugView_FormClosing;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)txtEventData).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnSend;
        private NumericUpDown txtEventData;
        private ComboBox cobEventName;
        private ListBox lvLog;
    }
}