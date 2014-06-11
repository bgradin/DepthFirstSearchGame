namespace GenericMapEditor
{
	partial class frmAddTile
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
			this.btnAddTile = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.graphicFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.graphicTilePreview = new GenericMapEditor.InterpolatedPictureBox();
			this.btnSelectGraphicTile = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.graphicTilePreview)).BeginInit();
			this.SuspendLayout();
			// 
			// btnAddTile
			// 
			this.btnAddTile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddTile.Location = new System.Drawing.Point(96, 213);
			this.btnAddTile.Name = "btnAddTile";
			this.btnAddTile.Size = new System.Drawing.Size(66, 23);
			this.btnAddTile.TabIndex = 1;
			this.btnAddTile.Text = "Add Tile";
			this.btnAddTile.UseVisualStyleBackColor = true;
			this.btnAddTile.Click += new System.EventHandler(this.btnAddTile_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(174, 213);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// graphicFileDialog
			// 
			this.graphicFileDialog.DefaultExt = "bmp";
			this.graphicFileDialog.Filter = "Graphic files|*.*";
			this.graphicFileDialog.SupportMultiDottedExtensions = true;
			this.graphicFileDialog.Title = "Select an image";
			// 
			// graphicTilePreview
			// 
			this.graphicTilePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.graphicTilePreview.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
			this.graphicTilePreview.Location = new System.Drawing.Point(63, 31);
			this.graphicTilePreview.Name = "graphicTilePreview";
			this.graphicTilePreview.Size = new System.Drawing.Size(128, 128);
			this.graphicTilePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.graphicTilePreview.TabIndex = 4;
			this.graphicTilePreview.TabStop = false;
			// 
			// btnSelectGraphicTile
			// 
			this.btnSelectGraphicTile.Location = new System.Drawing.Point(76, 165);
			this.btnSelectGraphicTile.Name = "btnSelectGraphicTile";
			this.btnSelectGraphicTile.Size = new System.Drawing.Size(102, 23);
			this.btnSelectGraphicTile.TabIndex = 3;
			this.btnSelectGraphicTile.Text = "Select Graphic";
			this.btnSelectGraphicTile.UseVisualStyleBackColor = true;
			this.btnSelectGraphicTile.Click += new System.EventHandler(this.btnSelectGraphicTile_Click);
			// 
			// frmAddTile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(261, 248);
			this.Controls.Add(this.graphicTilePreview);
			this.Controls.Add(this.btnSelectGraphicTile);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAddTile);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAddTile";
			this.ShowInTaskbar = false;
			this.Text = "Add Tile";
			((System.ComponentModel.ISupportInitialize)(this.graphicTilePreview)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnAddTile;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.OpenFileDialog graphicFileDialog;
		private InterpolatedPictureBox graphicTilePreview;
		private System.Windows.Forms.Button btnSelectGraphicTile;
	}
}