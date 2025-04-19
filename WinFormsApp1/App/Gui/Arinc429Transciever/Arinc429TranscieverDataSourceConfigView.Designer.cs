namespace AFooCockpit.App.Gui.DataSourceConfigForms
{
    partial class Arinc429TranscieverDataSourceConfigView
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
            label1 = new Label();
            label2 = new Label();
            txtVendorId = new TextBox();
            txtProductId = new TextBox();
            label3 = new Label();
            btnOk = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 61);
            label1.Name = "label1";
            label1.Size = new Size(96, 15);
            label1.TabIndex = 0;
            label1.Text = "Device Vendor ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 90);
            label2.Name = "label2";
            label2.Size = new Size(101, 15);
            label2.TabIndex = 1;
            label2.Text = "Device Product ID";
            // 
            // txtVendorId
            // 
            txtVendorId.Location = new Point(131, 53);
            txtVendorId.Name = "txtVendorId";
            txtVendorId.Size = new Size(254, 23);
            txtVendorId.TabIndex = 2;
            // 
            // txtProductId
            // 
            txtProductId.Location = new Point(131, 87);
            txtProductId.Name = "txtProductId";
            txtProductId.Size = new Size(254, 23);
            txtProductId.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 9);
            label3.Name = "label3";
            label3.Size = new Size(108, 15);
            label3.TabIndex = 4;
            label3.Text = "Device Information";
            // 
            // btnOk
            // 
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(310, 137);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 5;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(229, 137);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // CockpictConceptArinc429LsTranscieverDataSourceConfigView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(398, 174);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(label3);
            Controls.Add(txtProductId);
            Controls.Add(txtVendorId);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "CockpictConceptArinc429LsTranscieverDataSourceConfigView";
            Text = "CockpictConceptArinc429LsTranscieverDataSourceConfigView";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtVendorId;
        private TextBox txtProductId;
        private Label label3;
        private Button btnOk;
        private Button btnCancel;
    }
}