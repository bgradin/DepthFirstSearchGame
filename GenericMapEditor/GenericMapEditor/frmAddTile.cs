using System;
using System.Drawing;
using System.Windows.Forms;

using GameClassLibrary;

namespace GenericMapEditor
{
	public partial class frmAddTile : Form
	{
		Image GraphicTileGraphic { get; set; }
		public Tileset Tileset { get; set; }

		TileType tileType;

		public frmAddTile(TileType type)
		{
			tileType = type;

			InitializeComponent();
		}

		private void btnSelectGraphicTile_Click(object sender, EventArgs e)
		{
			graphicFileDialog.ShowDialog();

			try
			{
				GraphicTileGraphic = Image.FromFile(graphicFileDialog.FileName);

				// TODO: If the image is not the correct size, ask the user if they would still like to use it (resize it)
			}
			catch
			{
				MessageBox.Show("There was a problem loading the specified image.");
				GraphicTileGraphic = null;
				return;
			}

			graphicTilePreview.Image = GraphicTileGraphic;
		}

		private void btnAddTile_Click(object sender, EventArgs e)
		{
			if (GraphicTileGraphic == null)
				return;

			switch (tileType)
			{
				case TileType.Animated:
				case TileType.Graphic: // Graphic Tile
					Tileset.Images.Images.Add(GraphicTileGraphic);
					Tileset.Tiles.Add(new GraphicTile(Tileset.Images.Images.Count - 1));
					Close();
					break;
				case TileType.Special: // Special Tile
					Tileset.Images.Images.Add(GraphicTileGraphic);

					SpecialTile tile = new SpecialTile(-1, -1);
					tile.Graphic = Tileset.Images.Images.Count - 1;

					Tileset.Tiles.Add(tile);
					Close();
					break;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
