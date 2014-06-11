using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenericMapEditor
{
    public partial class frmNewMap : Form
    {
        public int mWidth { get; set; }
        public int mHeight { get; set; }
        public string mName { get; set; }

        public frmNewMap()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            int w, h;

            if (!int.TryParse(txtWidth.Text, out w) || !int.TryParse(txtHeight.Text, out h) || string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Error: enter integer values for width/height and a valid map name.");
                return;
            }

            mWidth = w;
            mHeight = h;
            mName = txtName.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
