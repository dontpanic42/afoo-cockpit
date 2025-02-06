namespace AFooCockpit.App.Gui
{
    partial class SerialDeviceGridView
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dgDeviceGrid = new DataGridView();
            toolStrip1 = new ToolStrip();
            tsbtnAddDevice = new ToolStripButton();
            tsbtnRemove = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)dgDeviceGrid).BeginInit();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dgDeviceGrid
            // 
            dgDeviceGrid.AllowUserToAddRows = false;
            dgDeviceGrid.AllowUserToDeleteRows = false;
            dgDeviceGrid.AllowUserToResizeRows = false;
            dgDeviceGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgDeviceGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgDeviceGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgDeviceGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgDeviceGrid.EnableHeadersVisualStyles = false;
            dgDeviceGrid.Location = new Point(3, 28);
            dgDeviceGrid.MultiSelect = false;
            dgDeviceGrid.Name = "dgDeviceGrid";
            dgDeviceGrid.RowHeadersVisible = false;
            dgDeviceGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgDeviceGrid.ShowEditingIcon = false;
            dgDeviceGrid.Size = new Size(694, 471);
            dgDeviceGrid.TabIndex = 0;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { tsbtnAddDevice, tsbtnRemove, toolStripSeparator1 });
            toolStrip1.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(700, 25);
            toolStrip1.TabIndex = 4;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnAddDevice
            // 
            tsbtnAddDevice.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnAddDevice.Image = Properties.Resources.add;
            tsbtnAddDevice.ImageTransparentColor = Color.Magenta;
            tsbtnAddDevice.Name = "tsbtnAddDevice";
            tsbtnAddDevice.Size = new Size(23, 22);
            tsbtnAddDevice.Text = "tsbtnAddDevice";
            tsbtnAddDevice.ToolTipText = "Add Device";
            tsbtnAddDevice.Click += tsbtnAddDevice_Click;
            // 
            // tsbtnRemove
            // 
            tsbtnRemove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbtnRemove.Image = Properties.Resources.remove;
            tsbtnRemove.ImageTransparentColor = Color.Magenta;
            tsbtnRemove.Name = "tsbtnRemove";
            tsbtnRemove.Size = new Size(23, 22);
            tsbtnRemove.Text = "tsbtnRemove";
            tsbtnRemove.ToolTipText = "Remove Device";
            tsbtnRemove.Click += tsbtnRemove_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // SerialDeviceGridView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(toolStrip1);
            Controls.Add(dgDeviceGrid);
            Name = "SerialDeviceGridView";
            Size = new Size(700, 499);
            ((System.ComponentModel.ISupportInitialize)dgDeviceGrid).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgDeviceGrid;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbtnRemove;
        private ToolStripButton tsbtnAddDevice;
        private ToolStripSeparator toolStripSeparator1;
    }
}
