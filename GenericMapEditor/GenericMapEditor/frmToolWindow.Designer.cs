namespace GenericMapEditor
{
    partial class frmToolWindow
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
            this.lvTools = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lvTools
            // 
            this.lvTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTools.HideSelection = false;
            this.lvTools.LabelWrap = false;
            this.lvTools.Location = new System.Drawing.Point(0, 0);
            this.lvTools.MultiSelect = false;
            this.lvTools.Name = "lvTools";
            this.lvTools.ShowGroups = false;
            this.lvTools.Size = new System.Drawing.Size(125, 176);
            this.lvTools.TabIndex = 0;
            this.lvTools.TileSize = new System.Drawing.Size(64, 64);
            this.lvTools.UseCompatibleStateImageBehavior = false;
            this.lvTools.View = System.Windows.Forms.View.Tile;
            // 
            // frmToolWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(125, 176);
            this.Controls.Add(this.lvTools);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmToolWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Tool Window";
            this.Load += new System.EventHandler(this.frmToolWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvTools;
    }
}