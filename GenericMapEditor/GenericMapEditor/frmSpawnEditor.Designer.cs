namespace GenericMapEditor
{
    partial class frmSpawnEditor
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
            this.lbPairs = new System.Windows.Forms.ListBox();
            this.txtSpawnID = new System.Windows.Forms.TextBox();
            this.lblID = new System.Windows.Forms.Label();
            this.btnAddPair = new System.Windows.Forms.Button();
            this.btnRemovePair = new System.Windows.Forms.Button();
            this.lblIdFreqPair = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbSpawnList = new System.Windows.Forms.ListBox();
            this.lblSpawnList = new System.Windows.Forms.Label();
            this.txtPID = new System.Windows.Forms.TextBox();
            this.lblPID = new System.Windows.Forms.Label();
            this.txtFreq = new System.Windows.Forms.TextBox();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.btnUpdatePair = new System.Windows.Forms.Button();
            this.btnAddSpawn = new System.Windows.Forms.Button();
            this.btnRemoveSpawn = new System.Windows.Forms.Button();
            this.pnlData = new System.Windows.Forms.Panel();
            this.pnlData.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbPairs
            // 
            this.lbPairs.FormattingEnabled = true;
            this.lbPairs.Location = new System.Drawing.Point(13, 18);
            this.lbPairs.Name = "lbPairs";
            this.lbPairs.Size = new System.Drawing.Size(125, 108);
            this.lbPairs.TabIndex = 8;
            this.lbPairs.SelectedIndexChanged += new System.EventHandler(this.lbPairs_SelectedIndexChanged);
            // 
            // txtSpawnID
            // 
            this.txtSpawnID.Location = new System.Drawing.Point(53, 176);
            this.txtSpawnID.Name = "txtSpawnID";
            this.txtSpawnID.Size = new System.Drawing.Size(84, 20);
            this.txtSpawnID.TabIndex = 3;
            this.txtSpawnID.TextChanged += new System.EventHandler(this.txtSpawnID_TextChanged);
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(12, 179);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(21, 13);
            this.lblID.TabIndex = 2;
            this.lblID.Text = "ID:";
            // 
            // btnAddPair
            // 
            this.btnAddPair.Enabled = false;
            this.btnAddPair.Location = new System.Drawing.Point(13, 188);
            this.btnAddPair.Name = "btnAddPair";
            this.btnAddPair.Size = new System.Drawing.Size(60, 23);
            this.btnAddPair.TabIndex = 13;
            this.btnAddPair.Text = "Add";
            this.btnAddPair.UseVisualStyleBackColor = true;
            this.btnAddPair.Click += new System.EventHandler(this.btnAddPair_Click);
            // 
            // btnRemovePair
            // 
            this.btnRemovePair.Enabled = false;
            this.btnRemovePair.Location = new System.Drawing.Point(79, 188);
            this.btnRemovePair.Name = "btnRemovePair";
            this.btnRemovePair.Size = new System.Drawing.Size(59, 23);
            this.btnRemovePair.TabIndex = 14;
            this.btnRemovePair.Text = "Remove";
            this.btnRemovePair.UseVisualStyleBackColor = true;
            this.btnRemovePair.Click += new System.EventHandler(this.btnRemovePair_Click);
            // 
            // lblIdFreqPair
            // 
            this.lblIdFreqPair.AutoSize = true;
            this.lblIdFreqPair.Location = new System.Drawing.Point(10, 2);
            this.lblIdFreqPair.Name = "lblIdFreqPair";
            this.lblIdFreqPair.Size = new System.Drawing.Size(105, 13);
            this.lblIdFreqPair.TabIndex = 18;
            this.lblIdFreqPair.Text = "ID / Frequency Pairs";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(53, 202);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(84, 20);
            this.txtDisplayName.TabIndex = 5;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(9, 205);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Name:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(130, 264);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(211, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbSpawnList
            // 
            this.lbSpawnList.FormattingEnabled = true;
            this.lbSpawnList.Location = new System.Drawing.Point(12, 30);
            this.lbSpawnList.Name = "lbSpawnList";
            this.lbSpawnList.Size = new System.Drawing.Size(125, 134);
            this.lbSpawnList.TabIndex = 1;
            this.lbSpawnList.SelectedIndexChanged += new System.EventHandler(this.lbSpawnList_SelectedIndexChanged);
            // 
            // lblSpawnList
            // 
            this.lblSpawnList.AutoSize = true;
            this.lblSpawnList.Location = new System.Drawing.Point(9, 14);
            this.lblSpawnList.Name = "lblSpawnList";
            this.lblSpawnList.Size = new System.Drawing.Size(59, 13);
            this.lblSpawnList.TabIndex = 0;
            this.lblSpawnList.Text = "Spawn List";
            // 
            // txtPID
            // 
            this.txtPID.Location = new System.Drawing.Point(88, 132);
            this.txtPID.Name = "txtPID";
            this.txtPID.Size = new System.Drawing.Size(50, 20);
            this.txtPID.TabIndex = 10;
            this.txtPID.TextChanged += new System.EventHandler(this.txtPID_TextChanged);
            // 
            // lblPID
            // 
            this.lblPID.AutoSize = true;
            this.lblPID.Location = new System.Drawing.Point(13, 135);
            this.lblPID.Name = "lblPID";
            this.lblPID.Size = new System.Drawing.Size(69, 13);
            this.lblPID.TabIndex = 9;
            this.lblPID.Text = "Pokemon ID:";
            // 
            // txtFreq
            // 
            this.txtFreq.Location = new System.Drawing.Point(88, 158);
            this.txtFreq.Name = "txtFreq";
            this.txtFreq.Size = new System.Drawing.Size(50, 20);
            this.txtFreq.TabIndex = 12;
            this.txtFreq.TextChanged += new System.EventHandler(this.txtPID_TextChanged);
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(13, 161);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(60, 13);
            this.lblFrequency.TabIndex = 11;
            this.lblFrequency.Text = "Frequency:";
            // 
            // btnUpdatePair
            // 
            this.btnUpdatePair.Enabled = false;
            this.btnUpdatePair.Location = new System.Drawing.Point(13, 217);
            this.btnUpdatePair.Name = "btnUpdatePair";
            this.btnUpdatePair.Size = new System.Drawing.Size(125, 23);
            this.btnUpdatePair.TabIndex = 15;
            this.btnUpdatePair.Text = "Update Selected";
            this.btnUpdatePair.UseVisualStyleBackColor = true;
            this.btnUpdatePair.Click += new System.EventHandler(this.btnUpdatePair_Click);
            // 
            // btnAddSpawn
            // 
            this.btnAddSpawn.Enabled = false;
            this.btnAddSpawn.Location = new System.Drawing.Point(12, 229);
            this.btnAddSpawn.Name = "btnAddSpawn";
            this.btnAddSpawn.Size = new System.Drawing.Size(60, 23);
            this.btnAddSpawn.TabIndex = 6;
            this.btnAddSpawn.Text = "Add";
            this.btnAddSpawn.UseVisualStyleBackColor = true;
            this.btnAddSpawn.Click += new System.EventHandler(this.btnAddSpawn_Click);
            // 
            // btnRemoveSpawn
            // 
            this.btnRemoveSpawn.Location = new System.Drawing.Point(78, 229);
            this.btnRemoveSpawn.Name = "btnRemoveSpawn";
            this.btnRemoveSpawn.Size = new System.Drawing.Size(59, 23);
            this.btnRemoveSpawn.TabIndex = 7;
            this.btnRemoveSpawn.Text = "Remove";
            this.btnRemoveSpawn.UseVisualStyleBackColor = true;
            this.btnRemoveSpawn.Click += new System.EventHandler(this.btnRemoveSpawn_Click);
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.lbPairs);
            this.pnlData.Controls.Add(this.lblIdFreqPair);
            this.pnlData.Controls.Add(this.lblPID);
            this.pnlData.Controls.Add(this.btnUpdatePair);
            this.pnlData.Controls.Add(this.btnRemovePair);
            this.pnlData.Controls.Add(this.txtPID);
            this.pnlData.Controls.Add(this.txtFreq);
            this.pnlData.Controls.Add(this.lblFrequency);
            this.pnlData.Controls.Add(this.btnAddPair);
            this.pnlData.Enabled = false;
            this.pnlData.Location = new System.Drawing.Point(143, 12);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(143, 246);
            this.pnlData.TabIndex = 19;
            // 
            // frmSpawnEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 299);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRemoveSpawn);
            this.Controls.Add(this.btnAddSpawn);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtDisplayName);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.txtSpawnID);
            this.Controls.Add(this.lblSpawnList);
            this.Controls.Add(this.lbSpawnList);
            this.Controls.Add(this.pnlData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSpawnEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Edit Spawn Data";
            this.pnlData.ResumeLayout(false);
            this.pnlData.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbPairs;
        private System.Windows.Forms.TextBox txtSpawnID;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button btnAddPair;
        private System.Windows.Forms.Button btnRemovePair;
        private System.Windows.Forms.Label lblIdFreqPair;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbSpawnList;
        private System.Windows.Forms.Label lblSpawnList;
        private System.Windows.Forms.TextBox txtPID;
        private System.Windows.Forms.Label lblPID;
        private System.Windows.Forms.TextBox txtFreq;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Button btnUpdatePair;
        private System.Windows.Forms.Button btnAddSpawn;
        private System.Windows.Forms.Button btnRemoveSpawn;
        private System.Windows.Forms.Panel pnlData;
    }
}