﻿namespace AFooCockpit.App.Gui
{
    partial class DeviceGridView
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
            dgDeviceGrid = new DataGridView();
            btnAddDevice = new Button();
            btnSave = new Button();
            btnRemoveDevice = new Button();
            ((System.ComponentModel.ISupportInitialize)dgDeviceGrid).BeginInit();
            SuspendLayout();
            // 
            // dgDeviceGrid
            // 
            dgDeviceGrid.AllowUserToResizeColumns = false;
            dgDeviceGrid.AllowUserToResizeRows = false;
            dgDeviceGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgDeviceGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgDeviceGrid.Location = new Point(3, 32);
            dgDeviceGrid.MultiSelect = false;
            dgDeviceGrid.Name = "dgDeviceGrid";
            dgDeviceGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgDeviceGrid.Size = new Size(465, 241);
            dgDeviceGrid.TabIndex = 0;
            // 
            // btnAddDevice
            // 
            btnAddDevice.ImageAlign = ContentAlignment.TopCenter;
            btnAddDevice.Location = new Point(3, 3);
            btnAddDevice.Name = "btnAddDevice";
            btnAddDevice.Size = new Size(75, 23);
            btnAddDevice.TabIndex = 1;
            btnAddDevice.Text = "Add Device";
            btnAddDevice.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAddDevice.UseVisualStyleBackColor = true;
            btnAddDevice.Click += btnAddDevice_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(390, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnRemoveDevice
            // 
            btnRemoveDevice.Location = new Point(84, 3);
            btnRemoveDevice.Name = "btnRemoveDevice";
            btnRemoveDevice.Size = new Size(110, 23);
            btnRemoveDevice.TabIndex = 3;
            btnRemoveDevice.Text = "Remove Device";
            btnRemoveDevice.UseVisualStyleBackColor = true;
            btnRemoveDevice.Click += btnRemoveDevice_Click;
            // 
            // DeviceGridView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnRemoveDevice);
            Controls.Add(btnSave);
            Controls.Add(btnAddDevice);
            Controls.Add(dgDeviceGrid);
            Name = "DeviceGridView";
            Size = new Size(468, 276);
            Load += DeviceGridView_Load;
            ((System.ComponentModel.ISupportInitialize)dgDeviceGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgDeviceGrid;
        private Button btnAddDevice;
        private Button btnSave;
        private Button btnRemoveDevice;
    }
}
