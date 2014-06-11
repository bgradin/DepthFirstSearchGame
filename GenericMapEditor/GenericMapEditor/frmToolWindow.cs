using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XNAGraphics = Microsoft.Xna.Framework.Graphics;

using GameClassLibrary;

namespace GenericMapEditor
{
    public partial class frmToolWindow : Form
    {
        ImageList images;

        public int SelIndex { get { return this.lvTools.SelectedIndices[0]; } }

        public frmToolWindow(List<XNAGraphics.Texture2D> textures)
        {
            InitializeComponent();

            images = new ImageList();
			images.ImageSize = new Size(EditorConst.DEFAULT_TILE_SIZE, EditorConst.DEFAULT_TILE_SIZE);
            images.ColorDepth = ColorDepth.Depth24Bit;
            
            foreach (XNAGraphics.Texture2D t in textures)
            {
                System.IO.MemoryStream mem = new System.IO.MemoryStream();
                t.SaveAsPng(mem, t.Width, t.Height);
                images.Images.Add(Image.FromStream(mem));
                mem.Close();
            }
        }

        public frmToolWindow(List<Image> textures)
        {
            InitializeComponent();

            images = new ImageList();
			images.ImageSize = new Size(EditorConst.DEFAULT_TILE_SIZE, EditorConst.DEFAULT_TILE_SIZE);
            images.ColorDepth = ColorDepth.Depth24Bit;

            foreach (Image i in textures)
            {
                images.Images.Add(i);
            }
        }

        private void frmToolWindow_Load(object sender, EventArgs e)
        {
            lvTools.SmallImageList = images;
            lvTools.LargeImageList = images;

            for(int i = 0; i < images.Images.Count; ++i)
                lvTools.Items.Add("", i);

            lvTools.SelectedIndices.Add(0);
        }
    }
}
