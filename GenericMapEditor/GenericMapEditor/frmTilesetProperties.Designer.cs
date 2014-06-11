namespace GenericMapEditor
{
	partial class frmTilesetProperties
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTilesetProperties));
			this.btnAddTile = new System.Windows.Forms.Button();
			this.btnRemoveTile = new System.Windows.Forms.Button();
			this.btnChangeGraphic = new System.Windows.Forms.Button();
			this.btnUp = new System.Windows.Forms.Button();
			this.btnDown = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.lblSize = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.listView1 = new System.Windows.Forms.ListView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.listView2 = new System.Windows.Forms.ListView();
			this.tilePreview = new GenericMapEditor.InterpolatedPictureBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tilePreview)).BeginInit();
			this.SuspendLayout();
			// 
			// btnAddTile
			// 
			this.btnAddTile.Location = new System.Drawing.Point(276, 146);
			this.btnAddTile.Name = "btnAddTile";
			this.btnAddTile.Size = new System.Drawing.Size(101, 23);
			this.btnAddTile.TabIndex = 2;
			this.btnAddTile.Text = "Add Tile";
			this.btnAddTile.UseVisualStyleBackColor = true;
			this.btnAddTile.Click += new System.EventHandler(this.btnAddTile_Click);
			// 
			// btnRemoveTile
			// 
			this.btnRemoveTile.Enabled = false;
			this.btnRemoveTile.Location = new System.Drawing.Point(276, 176);
			this.btnRemoveTile.Name = "btnRemoveTile";
			this.btnRemoveTile.Size = new System.Drawing.Size(101, 23);
			this.btnRemoveTile.TabIndex = 3;
			this.btnRemoveTile.Text = "Remove Tile";
			this.btnRemoveTile.UseVisualStyleBackColor = true;
			this.btnRemoveTile.Click += new System.EventHandler(this.btnRemoveTile_Click);
			// 
			// btnChangeGraphic
			// 
			this.btnChangeGraphic.Enabled = false;
			this.btnChangeGraphic.Location = new System.Drawing.Point(276, 206);
			this.btnChangeGraphic.Name = "btnChangeGraphic";
			this.btnChangeGraphic.Size = new System.Drawing.Size(101, 23);
			this.btnChangeGraphic.TabIndex = 4;
			this.btnChangeGraphic.Text = "Change Graphic";
			this.btnChangeGraphic.UseVisualStyleBackColor = true;
			this.btnChangeGraphic.Click += new System.EventHandler(this.btnChangeGraphic_Click);
			// 
			// btnUp
			// 
			this.btnUp.Enabled = false;
			this.btnUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUp.Location = new System.Drawing.Point(249, 146);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(21, 38);
			this.btnUp.TabIndex = 5;
			this.btnUp.Text = "↑";
			this.btnUp.UseVisualStyleBackColor = true;
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// btnDown
			// 
			this.btnDown.Enabled = false;
			this.btnDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDown.Location = new System.Drawing.Point(249, 190);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(21, 38);
			this.btnDown.TabIndex = 6;
			this.btnDown.Text = "↓";
			this.btnDown.UseVisualStyleBackColor = true;
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 273);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Name:";
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(56, 270);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(204, 20);
			this.tbName.TabIndex = 8;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(266, 268);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(52, 23);
			this.btnOK.TabIndex = 9;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(251, 242);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Tile size:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(303, 239);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(36, 20);
			this.textBox1.TabIndex = 11;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// lblSize
			// 
			this.lblSize.AutoSize = true;
			this.lblSize.Location = new System.Drawing.Point(345, 242);
			this.lblSize.Name = "lblSize";
			this.lblSize.Size = new System.Drawing.Size(12, 13);
			this.lblSize.TabIndex = 12;
			this.lblSize.Text = "x";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(324, 268);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(53, 23);
			this.btnCancel.TabIndex = 13;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(231, 252);
			this.tabControl1.TabIndex = 14;
			this.tabControl1.SelectedIndexChanged += tabControl1_TabIndexChanged;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.listView1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(223, 226);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Graphic Tiles";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(1, 2);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(219, 223);
			this.listView1.TabIndex = 1;
			this.listView1.TileSize = new System.Drawing.Size(32, 32);
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Tile;
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.listView2);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(223, 226);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Special Tiles";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// listView2
			// 
			this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listView2.HideSelection = false;
			this.listView2.Location = new System.Drawing.Point(1, 2);
			this.listView2.MultiSelect = false;
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(219, 223);
			this.listView2.TabIndex = 2;
			this.listView2.TileSize = new System.Drawing.Size(32, 32);
			this.listView2.UseCompatibleStateImageBehavior = false;
			this.listView2.View = System.Windows.Forms.View.Tile;
			this.listView2.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
			// 
			// tilePreview
			// 
			this.tilePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tilePreview.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			this.tilePreview.Location = new System.Drawing.Point(249, 12);
			this.tilePreview.Name = "tilePreview";
			this.tilePreview.Size = new System.Drawing.Size(128, 128);
			this.tilePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.tilePreview.TabIndex = 1;
			this.tilePreview.TabStop = false;
			// 
			// frmTilesetProperties
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(389, 296);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblSize);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnDown);
			this.Controls.Add(this.btnUp);
			this.Controls.Add(this.btnChangeGraphic);
			this.Controls.Add(this.btnRemoveTile);
			this.Controls.Add(this.btnAddTile);
			this.Controls.Add(this.tilePreview);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(405, 278);
			this.Name = "frmTilesetProperties";
			this.Text = "Tileset Properties";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tilePreview)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private InterpolatedPictureBox tilePreview;
		private System.Windows.Forms.Button btnAddTile;
		private System.Windows.Forms.Button btnRemoveTile;
		private System.Windows.Forms.Button btnChangeGraphic;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label lblSize;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		public System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.TabPage tabPage2;
		public System.Windows.Forms.ListView listView2;

	}
}