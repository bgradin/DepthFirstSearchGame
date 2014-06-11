namespace GenericMapEditor
{
    partial class frmMapProperties
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
            this.grpMapSize = new System.Windows.Forms.GroupBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpMapName = new System.Windows.Forms.GroupBox();
            this.txtMapName = new System.Windows.Forms.TextBox();
            this.grpWardID = new System.Windows.Forms.GroupBox();
            this.txtWarpID = new System.Windows.Forms.TextBox();
            this.cmbMapType = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpPlayerSpawn = new System.Windows.Forms.GroupBox();
            this.txtSpawnY = new System.Windows.Forms.TextBox();
            this.txtSpawnX = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grpMapSize.SuspendLayout();
            this.grpMapName.SuspendLayout();
            this.grpWardID.SuspendLayout();
            this.grpPlayerSpawn.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpMapSize
            // 
            this.grpMapSize.Controls.Add(this.txtHeight);
            this.grpMapSize.Controls.Add(this.txtWidth);
            this.grpMapSize.Controls.Add(this.label2);
            this.grpMapSize.Controls.Add(this.label1);
            this.grpMapSize.Location = new System.Drawing.Point(12, 12);
            this.grpMapSize.Name = "grpMapSize";
            this.grpMapSize.Size = new System.Drawing.Size(124, 75);
            this.grpMapSize.TabIndex = 0;
            this.grpMapSize.TabStop = false;
            this.grpMapSize.Text = "Map Size";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(57, 44);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(61, 20);
            this.txtHeight.TabIndex = 3;
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(57, 18);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(61, 20);
            this.txtWidth.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Height";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Width";
            // 
            // grpMapName
            // 
            this.grpMapName.Controls.Add(this.txtMapName);
            this.grpMapName.Location = new System.Drawing.Point(12, 93);
            this.grpMapName.Name = "grpMapName";
            this.grpMapName.Size = new System.Drawing.Size(124, 48);
            this.grpMapName.TabIndex = 1;
            this.grpMapName.TabStop = false;
            this.grpMapName.Text = "Map Name";
            // 
            // txtMapName
            // 
            this.txtMapName.Location = new System.Drawing.Point(6, 19);
            this.txtMapName.Name = "txtMapName";
            this.txtMapName.Size = new System.Drawing.Size(112, 20);
            this.txtMapName.TabIndex = 0;
            // 
            // grpWardID
            // 
            this.grpWardID.Controls.Add(this.txtWarpID);
            this.grpWardID.Location = new System.Drawing.Point(12, 147);
            this.grpWardID.Name = "grpWardID";
            this.grpWardID.Size = new System.Drawing.Size(124, 52);
            this.grpWardID.TabIndex = 2;
            this.grpWardID.TabStop = false;
            this.grpWardID.Text = "Warp ID";
            // 
            // txtWarpID
            // 
            this.txtWarpID.Location = new System.Drawing.Point(6, 19);
            this.txtWarpID.Name = "txtWarpID";
            this.txtWarpID.Size = new System.Drawing.Size(112, 20);
            this.txtWarpID.TabIndex = 0;
            // 
            // cmbMapType
            // 
            this.cmbMapType.FormattingEnabled = true;
            this.cmbMapType.Location = new System.Drawing.Point(207, 71);
            this.cmbMapType.Name = "cmbMapType";
            this.cmbMapType.Size = new System.Drawing.Size(98, 21);
            this.cmbMapType.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(149, 205);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(230, 205);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpPlayerSpawn
            // 
            this.grpPlayerSpawn.Controls.Add(this.txtSpawnY);
            this.grpPlayerSpawn.Controls.Add(this.txtSpawnX);
            this.grpPlayerSpawn.Controls.Add(this.label3);
            this.grpPlayerSpawn.Controls.Add(this.label4);
            this.grpPlayerSpawn.Location = new System.Drawing.Point(142, 12);
            this.grpPlayerSpawn.Name = "grpPlayerSpawn";
            this.grpPlayerSpawn.Size = new System.Drawing.Size(163, 50);
            this.grpPlayerSpawn.TabIndex = 4;
            this.grpPlayerSpawn.TabStop = false;
            this.grpPlayerSpawn.Text = "Player Spawn (default)";
            // 
            // txtSpawnY
            // 
            this.txtSpawnY.Location = new System.Drawing.Point(104, 20);
            this.txtSpawnY.Name = "txtSpawnY";
            this.txtSpawnY.Size = new System.Drawing.Size(36, 20);
            this.txtSpawnY.TabIndex = 3;
            // 
            // txtSpawnX
            // 
            this.txtSpawnX.Location = new System.Drawing.Point(42, 19);
            this.txtSpawnX.Name = "txtSpawnX";
            this.txtSpawnX.Size = new System.Drawing.Size(36, 20);
            this.txtSpawnX.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(84, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(146, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Map Type";
            // 
            // frmMapProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(317, 240);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.grpPlayerSpawn);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbMapType);
            this.Controls.Add(this.grpWardID);
            this.Controls.Add(this.grpMapName);
            this.Controls.Add(this.grpMapSize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMapProperties";
            this.ShowInTaskbar = false;
            this.Text = "Map Properties";
            this.Load += new System.EventHandler(this.frmMapProperties_Load);
            this.grpMapSize.ResumeLayout(false);
            this.grpMapSize.PerformLayout();
            this.grpMapName.ResumeLayout(false);
            this.grpMapName.PerformLayout();
            this.grpWardID.ResumeLayout(false);
            this.grpWardID.PerformLayout();
            this.grpPlayerSpawn.ResumeLayout(false);
            this.grpPlayerSpawn.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpMapSize;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpMapName;
        private System.Windows.Forms.TextBox txtMapName;
        private System.Windows.Forms.GroupBox grpWardID;
        private System.Windows.Forms.TextBox txtWarpID;
        private System.Windows.Forms.ComboBox cmbMapType;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpPlayerSpawn;
        private System.Windows.Forms.TextBox txtSpawnY;
        private System.Windows.Forms.TextBox txtSpawnX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}