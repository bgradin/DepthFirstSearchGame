using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using GameClassLibrary;

namespace GenericMapEditor
{
	public partial class frmTilesetProperties : Form
	{
		public Tileset Tiles { get; set; }
		public Map Map { get; set; }

		public frmTilesetProperties(Tileset ts, Map m)
		{
			InitializeComponent();

			accepted = false;
			m_originalTileset = new Tileset(ts);
			Tiles = new Tileset(ts);
			m_originalMap = m != null ? new Map(m) : null;
			Map = m != null ? new Map(m) : null;
			m_sourceImages = new List<Image>();
			for (int i = 0; i < ts.Images.Images.Count; i++)
				m_sourceImages.Add((Image)ts.Images.Images[i].Clone());

			listView1.LargeImageList = ts.Images;
			listView2.LargeImageList = ts.Images;

			if (listView1.Items.Count > 0)
				listView1.SelectedIndices.Add(0);
			if (listView2.Items.Count > 0)
				listView2.SelectedIndices.Add(0);

			listView1.RefreshContents(Tiles, TileType.Graphic);
			listView2.RefreshContents(Tiles, TileType.Special);

			Shown += (o, e) =>
			{
				tbName.Text = Tiles.Name;
				textBox1.Text = Tiles.Images.ImageSize.Width.ToString();
			};
			tbName.TextChanged += (o, e) =>
			{
				Tiles.Name = tbName.Text;
				Tiles.FileUpToDate = false;
			};

			RevertToSourceImageList();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (Tiles.Tiles.Count == 0)
			{
				MessageBox.Show("There must be at least one tile in your tileset.", "Error");
				e.Cancel = true;
			}

			if (!accepted && !e.Cancel)
				RevertAllChanges();

			base.OnClosing(e);
		}

		// This is one of the ugliest functions I've ever written
		private void ApplyChange(int source, int dest)
		{
			if (source == dest)
				return;

			Tiles.FileUpToDate = false;

			// Update integer value for graphic of tiles
			Tile destTile = Tiles.Tiles[dest];
			if (destTile is GraphicTile)
				(destTile as GraphicTile).Graphic = source;
			Tile sourceTile = Tiles.Tiles[source];
			if (sourceTile is GraphicTile)
				(sourceTile as GraphicTile).Graphic = dest;

			// Swap images in imagelist and source images
			var tmp = Tiles.Images.Images[source];
			Tiles.Images.Images[source] = Tiles.Images.Images[dest];
			Tiles.Images.Images[dest] = tmp;
			var tmp2 = Tiles.Tiles[source];
			Tiles.Tiles[source] = Tiles.Tiles[dest];
			Tiles.Tiles[dest] = tmp2;
			var tmp3 = m_sourceImages[source];
			m_sourceImages[source] = m_sourceImages[dest];
			m_sourceImages[dest] = tmp3;

			listView1.RefreshContents(Tiles, TileType.Graphic);
			listView1.SelectedIndices.Clear();
			if (listView1.Items.Count > 0)
				listView1.SelectedIndices.Add(0);

			listView2.RefreshContents(Tiles, TileType.Special);
			listView2.SelectedIndices.Clear();
			if (listView2.Items.Count > 0)
				listView2.SelectedIndices.Add(0);

			if (Map != null)
			{
				((Action)(() =>
				{
					foreach (Tile tile in Map.Layers[(int)LAYERS.Graphic].Values)
					{
						if (tile is GraphicTile)
						{
							GraphicTile gTile = tile as GraphicTile;

							if (gTile.Graphic == source)
								gTile.Graphic = dest;
							else if (gTile.Graphic == dest)
								gTile.Graphic = source;
						}
					}
				})).BeginInvoke(null, null);
			}
		}

		private void tabControl1_TabIndexChanged(object sender, System.EventArgs e)
		{
			if (tabControl1.SelectedIndex == 0)
				listView1_SelectedIndexChanged(null, null);
			else if (tabControl1.SelectedIndex == 1)
				listView2_SelectedIndexChanged(null, null);
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedIndices.Count == 0)
			{
				tilePreview.Image = new Bitmap(1, 1);
				btnUp.Enabled = false;
				btnDown.Enabled = false;
				btnRemoveTile.Enabled = false;
				btnChangeGraphic.Enabled = false;
			}
			else if (listView1.LargeImageList != null)
			{
				tilePreview.Image = listView1.LargeImageList.Images[Tiles.CorrectedTileIndex<GraphicTile>(listView1.SelectedIndices[0])];
				if (listView1.SelectedIndices[0] > 0)
					btnUp.Enabled = true;
				if (listView1.SelectedIndices[0] < listView1.Items.Count - 1)
					btnDown.Enabled = true;
				btnRemoveTile.Enabled = listView1.Items.Count > 1;
				btnChangeGraphic.Enabled = true;
			}
		}

		private void listView2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView2.SelectedIndices.Count == 0)
			{
				tilePreview.Image = new Bitmap(1, 1);
				btnUp.Enabled = false;
				btnDown.Enabled = false;
				btnRemoveTile.Enabled = false;
				btnChangeGraphic.Enabled = false;
			}
			else if (listView2.LargeImageList != null)
			{
				tilePreview.Image = listView1.LargeImageList.Images[Tiles.CorrectedTileIndex<SpecialTile>(listView2.SelectedIndices[0])];
				if (listView2.SelectedIndices[0] > 0)
					btnUp.Enabled = true;
				if (listView2.SelectedIndices[0] < listView2.Items.Count - 1)
					btnDown.Enabled = true;
				btnRemoveTile.Enabled = listView2.Items.Count > 1;
				btnChangeGraphic.Enabled = true;
			}
		}

		private void btnAddTile_Click(object sender, EventArgs e)
		{
			frmAddTile addTileForm = new frmAddTile(tabControl1.SelectedIndex == 0 ? TileType.Graphic : TileType.Special);

			int i = Tiles.Images.Images.Count;

			addTileForm.Tileset = Tiles;
			addTileForm.ShowDialog();

			if (i < Tiles.Images.Images.Count)
				m_sourceImages.Add((Image)Tiles.Images.Images[Tiles.Images.Images.Count - 1].Clone());

			Tiles.FileUpToDate = false;

			listView1.RefreshContents(Tiles, TileType.Graphic);
			listView1.SelectedIndices.Clear();
			if (listView1.Items.Count > 0)
				listView1.SelectedIndices.Add(0);

			listView2.RefreshContents(Tiles, TileType.Special);
			listView2.SelectedIndices.Clear();
			if (listView2.Items.Count > 0)
				listView2.SelectedIndices.Add(0);
		}

		private void btnRemoveTile_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count == 0 && listView2.SelectedItems.Count == 0)
				return;

			ListView selectedListView = tabControl1.SelectedIndex == 0 ? listView1 : listView2;

			// Make sure we don't mess up the map
			if (Map != null)
			{
				int count = Map.Layers[(int)LAYERS.Graphic].Values.Where(i => (i as GraphicTile).Graphic == selectedListView.SelectedIndices[0]).Count(i => true);

				if (count > 0)
				{
					if (MessageBox.Show("This will erase some tiles from the current map. Continue?", "", MessageBoxButtons.YesNo) == DialogResult.No)
						return;
				}

				// Remove the removed tile from the map
				foreach (Tile tile in Map.Layers[(int)LAYERS.Graphic].Values)
				{
					GraphicTile gTile = tile as GraphicTile;

					if (gTile.Graphic == selectedListView.SelectedIndices[0])
						gTile.Graphic = 0;
				}
			}

			// Remove the tile from everything
			Tiles.Tiles.RemoveAt(selectedListView.SelectedIndices[0]);
			Tiles.Images.Images.RemoveAt(selectedListView.SelectedIndices[0]);
			m_sourceImages.RemoveAt(selectedListView.SelectedIndices[0]);

			// Make sure graphic tiles are still pointing to the right things
			for (int i = selectedListView.SelectedIndices[0]; i < Tiles.Tiles.Count; i++)
			{
				if (Tiles.Tiles[i] is GraphicTile)
				{
					GraphicTile gt = Tiles.Tiles[i] as GraphicTile;
					gt.Graphic--;
				}
			}

			// Set up selected index
			int tmp = selectedListView.SelectedIndices[0];
			listView1.RefreshContents(Tiles, TileType.Graphic);
			listView2.RefreshContents(Tiles, TileType.Special);
			selectedListView.SelectedIndices.Clear();
			if (tmp > selectedListView.Items.Count - 1)
				selectedListView.SelectedIndices.Add(selectedListView.Items.Count - 1);
			else
				selectedListView.SelectedIndices.Add(tmp);

			Tiles.FileUpToDate = false;
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			ListView selectedListView = tabControl1.SelectedIndex == 0 ? listView1 : listView2;

			if (selectedListView.SelectedItems.Count > 0 && selectedListView.SelectedIndices[0] > 0)
				ApplyChange(selectedListView.SelectedIndices[0], selectedListView.SelectedIndices[0] - 1);
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			ListView selectedListView = tabControl1.SelectedIndex == 0 ? listView1 : listView2;

			if (selectedListView.SelectedItems.Count > 0 && selectedListView.SelectedIndices[0] < selectedListView.Items.Count - 1)
				ApplyChange(selectedListView.SelectedIndices[0], selectedListView.SelectedIndices[0] + 1);
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			accepted = true;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			RevertAllChanges();
			Close();
		}

		private void btnChangeGraphic_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count == 0 && listView2.SelectedItems.Count == 0)
				return;

			ListView selectedListView = tabControl1.SelectedIndex == 0 ? listView1 : listView2;

			OpenFileDialog od = new OpenFileDialog();
			od.Filter = "Graphic files|*.*";
			od.Title = "Select a graphic";
			od.ShowDialog();

			if (od.FileName == "")
				return;

			TileType type = selectedListView == listView1 ? TileType.Graphic : TileType.Special;
			int index = selectedListView == listView1 ? Tiles.CorrectedTileIndex<GraphicTile>(selectedListView.SelectedIndices[0])
				: Tiles.CorrectedTileIndex<SpecialTile>(selectedListView.SelectedIndices[0]);
			int actualIndex = selectedListView.SelectedIndices[0];

			Image newImage = Image.FromFile(od.FileName);
			Tiles.Images.Images[index] = newImage;
			selectedListView.RefreshContents(Tiles, type);

			selectedListView.SelectedIndices.Clear();
			selectedListView.SelectedIndices.Add(actualIndex);
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			lblSize.Text = "x" + textBox1.Text;

			int size;
			if (!int.TryParse(textBox1.Text, out size))
				return;


			RevertToSourceImageList();
			Tiles.Images.ImageSize = new Size(size, size);
			listView1.TileSize = new Size(size + 10, size + 10);
			listView1.RefreshContents(Tiles, TileType.Graphic);
			listView2.TileSize = new Size(size + 10, size + 10);
			listView2.RefreshContents(Tiles, TileType.Special);
		}

		private void RevertAllChanges()
		{
			Tiles = new Tileset(m_originalTileset);
			Map = m_originalMap != null ? new Map(m_originalMap) : null;
		}

		private void RevertToSourceImageList()
		{
			Tiles.Images = new ImageList();
			Tiles.Images.ColorDepth = m_originalTileset.Images.ColorDepth;
			Tiles.Images.ImageSize = new Size(m_originalTileset.Images.ImageSize.Width, m_originalTileset.Images.ImageSize.Height);

			for (int i = 0; i < m_sourceImages.Count; i++)
				Tiles.Images.Images.Add((Image)m_sourceImages[i].Clone());
		}

		Tileset m_originalTileset;
		Map m_originalMap;
		List<Image> m_sourceImages;
		bool accepted;
	}
}
