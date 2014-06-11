using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GameClassLibrary;

namespace GenericMapEditor
{
	public enum PlaceState
	{
		SINGLEFILL,
		MULTIFILL,
		ERASER,
		PAN
	}

	public enum DragState
	{
		NoDrag,
		Horizontal,
		Vertical,
		Indeterminate
	}
	
	public partial class frmMain : Form
	{
		static readonly object lockObject = new object();
		static double[] zoomLevels = { .125, .25, .5, .75, 1, 1.5, 2, 2.5, 3, 4 };
		int zoomIndex = 4;
		bool loaded;

		Size size;

		bool m_fileOpen;
		Map m_openMap;
		MapBuffer m_buffer = new MapBuffer();

		Tileset tileset = new Tileset();

		PlaceState currentState = PlaceState.SINGLEFILL;
		Vector2 originalLocation = new Vector2(0, 0), originalAdjustment = new Vector2(0, 0);
		bool prevMousePressed = false;

		bool shiftKeyPressed = false, prevShift = false;
		int orig_x, orig_y;
		DragState dragdir = DragState.NoDrag;

		public frmMain()
		{
			InitializeComponent();

			mapViewer1.TileSize = 16;

			loaded = false;
			m_openMap = null;
			CloseDisableControls();

			Shown += (o, e) => loaded = true;
			ResizeEnd += WindowBoundsChanged;
			Resize += HandleMaximization;
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			mapViewer1.DrawImages = new List<Texture2D>();

			string defaultTilesetPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Resources\\default.tileset";

			if (!File.Exists(defaultTilesetPath))
			{
				if (MessageBox.Show("No default tileset found. Create one now?", "", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					Application.Exit();
					return;
				}

				EditTileset();
				if (tileset.Tiles.Count == 0)
				{
					Application.Exit();
					return;
				}
			}
			else
			{
				tileset.LoadFromFile(defaultTilesetPath);
				mapViewer1.TileSize = tileset.Images.ImageSize.Width;
				RegistryAccess.AddRecentFile(tileset.FileName, RegistrySetting.RecentTilesets);
				RefreshListViewer();
			}

			//set up graphics device to pass to the created content manager
			GraphicsDeviceService gds = GraphicsDeviceService.AddRef(Handle, ClientSize.Width, ClientSize.Height);
			ServiceContainer service = new ServiceContainer();
			service.AddService<IGraphicsDeviceService>(gds);
			ContentManager cont = new ContentManager(service, EditorConst.CONTENT_ROOT_DIR);

			Left = (int)RegistryAccess.GetValue(RegistrySetting.WindowX);
			Top = (int)RegistryAccess.GetValue(RegistrySetting.WindowY);

			Size = new Size((int)RegistryAccess.GetValue(RegistrySetting.WindowWidth), (int)RegistryAccess.GetValue(RegistrySetting.WindowHeight));

			//if (((int)RegistryAccess.GetValueOrDefault(RegistrySetting.WindowMaximized, EditorConst.WINDOW_DEFAULT_MAXIMIZATION)).ToBool())
				WindowState = FormWindowState.Maximized;
				//WindowState = FormWindowState.Maximized;
			//	WindowState = FormWindowState.Maximized;

			mapViewer1.GridText = cont.Load<Texture2D>("gridbox");
			mapViewer1.SelectorText = cont.Load<Texture2D>("selector");
			
			System.Resources.ResourceSet res = GenericMapEditor.Properties.Resources.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo("en-us"), true, true);

			for (int i = 0; i < (int)SpecialTileSpec.NUM_VALS; ++i)
				cmbSpecialType.Items.Add(Enum.GetName(typeof(SpecialTileSpec), (SpecialTileSpec)i));
			cmbSpecialType.SelectedIndex = 0;
			for (int i = (int)WarpAnim.NONE; i < (int)WarpAnim.NUM_VALS; ++i)
				cmbWarpAnim.Items.Add(Enum.GetName(typeof(WarpAnim), (WarpAnim)i));
			cmbWarpAnim.SelectedIndex = 0;

			RefreshRecentList(mnuFileRecent, RegistrySetting.RecentMaps);
			RefreshRecentList(recentToolStripMenuItem, RegistrySetting.RecentTilesets);
		}

		private void RefreshListViewer()
		{
			lvTiles.TileSize = new Size(tileset.Images.ImageSize.Width + 10, tileset.Images.ImageSize.Height + 10);
			lvTiles.RefreshContents(tileset, TileType.Graphic);

			lvTiles2.TileSize = new Size(tileset.Images.ImageSize.Width + 10, tileset.Images.ImageSize.Height + 10);
			lvTiles2.RefreshContents(tileset, TileType.Special);

			mapViewer1.DrawImages.Clear();
			foreach (Tile tile in tileset.Tiles)
			{
				if (tile is GraphicTile)
				{
					Image image = tileset.Images.Images[(tile as GraphicTile).Graphic];

					using (MemoryStream ms = new MemoryStream())
					{
						image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
						mapViewer1.DrawImages.Add(Texture2D.FromStream(mapViewer1.GraphicsDevice, ms));
					}
				}
			}
		}

		private void OpenEnableControls()
		{
			//helper method for enabling/showing appropriate controls when a file is opened/created
			m_fileOpen = true;
			mnuSave.Enabled = true;
			mnuSaveAs.Enabled = true;
			tsSave.Enabled = true;
			mnuClose.Enabled = true;

			mnuUndo.Enabled = false;
			mnuRedo.Enabled = false;
			m_buffer.Clear();
			m_buffer.AddState(m_openMap);

			mnuMap.Enabled = true;
			mnuView.Enabled = true;

			tslblFileName.Text = string.IsNullOrEmpty(m_openMap.FileName) ? "[unsaved file]" : m_openMap.FileName;
			tslblFileName.Visible = true;

			tsLblSelX.Text = (mapViewer1.SelectorPosition.X / mapViewer1.TileSize).ToString();
			tsLblSelX.Visible = true;

			tsLblSelY.Text = (mapViewer1.SelectorPosition.Y / mapViewer1.TileSize).ToString();
			tsLblSelY.Visible = true;

			tsLblNumTiles.Text = m_openMap.Layers[0].Count.ToString();
			tsLblNumTiles.Visible = true;

			tslblZoomLevel.Visible = true;

			cmbSpecialType.Enabled = true;
			cmbSpecialType_SelectedIndexChanged(null, null);

			mapViewer1.Refresh();
		}

		private void EditTileset()
		{
			frmTilesetProperties tilesetProperties = new frmTilesetProperties(tileset, m_openMap);

			if (loaded)
				tilesetProperties.ShowInTaskbar = false;

			tilesetProperties.FormClosing += (o, e) =>
			{
				frmTilesetProperties form = o as frmTilesetProperties;

				if (form == null)
					return;

				if (!form.Tiles.CheckAgainst(m_openMap))
				{
					if (MessageBox.Show("Some features of the current map don't work with this tileset.\n Would you like to close the current map?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
						mnuClose_Click(null, null);

					if (m_fileOpen)
						return;
				}

				tileset = form.Tiles;
				m_openMap = form.Map;
				mapViewer1.TileSize = form.Tiles.Images.ImageSize.Width;
			};

			tilesetProperties.ShowDialog();
			RefreshListViewer();
		}

		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			if (!tileset.FileUpToDate)
			{
				if (MessageBox.Show("Would you like to save the current tileset?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
					saveToolStripMenuItem_Click(null, null);
				else
					e.Cancel = true;
			}

			base.OnClosing(e);
		}

		private void CloseDisableControls()
		{
			//helper method for disabling/hiding appropriate controls when a file is closed
			m_fileOpen = false;
			mnuSave.Enabled = false;
			mnuSaveAs.Enabled = false;
			tsSave.Enabled = false;
			mnuClose.Enabled = false;

			mnuUndo.Enabled = false;
			mnuRedo.Enabled = false;

			mnuMap.Enabled = false;
			mnuView.Enabled = false;

			tslblFileName.Visible = false;
			tsLblSelX.Visible = false;
			tsLblSelY.Visible = false;
			tsLblNumTiles.Visible = false;
			tslblZoomLevel.Visible = false;

			cmbSpecialType.Enabled = false;
			cmbSpecialType_SelectedIndexChanged(null, null);

			m_buffer.Clear();
			mapViewer1.ClearMapData();
			mapViewer1.Refresh();
		}

		private void UpdateUndoRedo()
		{
			mnuUndo.Enabled = m_buffer.UndoReady;
			mnuRedo.Enabled = m_buffer.RedoReady;
		}

		void WindowBoundsChanged(object sender, EventArgs e)
		{
			if (size != Size)
			{
				size = Size;
				RegistryAccess.SetValue(RegistrySetting.WindowMaximized, false);
				RegistryAccess.SetValue(RegistrySetting.WindowWidth, ClientSize.Width);
				RegistryAccess.SetValue(RegistrySetting.WindowHeight, ClientSize.Height);
			}

			if (loaded)
			{
				RegistryAccess.SetValue(RegistrySetting.WindowX, Bounds.X);
				RegistryAccess.SetValue(RegistrySetting.WindowY, Bounds.Y);
			}
		}

		void HandleMaximization(object sender, EventArgs e)
		{
			if (WindowState == FormWindowState.Maximized)
				RegistryAccess.SetValue(RegistrySetting.WindowMaximized, true);
		}

		private bool PromptSaveChangesAndClose()
		{
			DialogResult res = MessageBox.Show("Save changes to open map?", "Save changes?", MessageBoxButtons.YesNoCancel);
			switch (res)
			{
				case System.Windows.Forms.DialogResult.Yes: mnuSave_Click(new object(), new EventArgs()); break;
				case System.Windows.Forms.DialogResult.No: break;
				case System.Windows.Forms.DialogResult.Cancel:
				default: return false;
			}

			m_fileOpen = false;
			m_openMap = null;
			mapViewer1.ClearMapData();
			mapViewer1.Refresh();

			return true;
		}

		private void SaveAsHelper(bool saveAs)
		{
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.AddExtension = true;
				sfd.CheckPathExists = true;
				sfd.DefaultExt = ".bmap";
				sfd.Filter = "Binary Map|*.bmap";
				sfd.OverwritePrompt = true;

				DialogResult dr = sfd.ShowDialog();
				if (dr == DialogResult.OK)
				{
					try
					{
						if (!m_openMap.Save(sfd.FileName, saveAs))
							MessageBox.Show("Error saving file!");
						else
						{
							m_buffer.UpdateFilePath(sfd.FileName);
							tslblFileName.Text = sfd.FileName;
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
			}

			RegistryAccess.AddRecentFile(m_openMap.FileName, RegistrySetting.RecentMaps);
			RefreshRecentList(mnuFileRecent, RegistrySetting.RecentMaps);
		}

		private bool CreateSpecialTile(out Tile toAdd)
		{
			toAdd = new SpecialTile(int.Parse(tsLblSelX.Text), int.Parse(tsLblSelY.Text));
			if (cmbSpecialType.SelectedIndex < 0 || cmbWarpAnim.SelectedIndex < 0 || lvTiles2.SelectedIndices.Count == 0)
				return false;
			SpecialTileSpec type = (SpecialTileSpec)cmbSpecialType.SelectedIndex;
			object[] paramList;
			if (type == SpecialTileSpec.WARP)
			{
				int outID, outX, outY;
				WarpAnim add = (WarpAnim)cmbWarpAnim.SelectedIndex;
				if (!int.TryParse(txtDestWarpX.Text, out outX) || !int.TryParse(txtDestWarpY.Text, out outY) || !int.TryParse(txtDestWarpMap.Text, out outID))
				{
					MessageBox.Show("Error: please enter valid values for X, Y, and Dest");
					return false;
				}
				paramList = new object[4];
				paramList[0] = outID; paramList[1] = outX; paramList[2] = outY; paramList[3] = add;
			}
			else
				paramList = null;

			(toAdd as SpecialTile).SetType(type, paramList);
			(toAdd as SpecialTile).Graphic = lvTiles2.SelectedIndices[0];
			return true;
		}

		private void FillTileGraphic(Tile tileToFill, Tile fillTile)
		{
			GraphicTile destTile = tileToFill as GraphicTile, sourceTile = fillTile as GraphicTile;

			if (destTile.Graphic == sourceTile.Graphic)
				return;

			int graphicToMatch = destTile.Graphic;

			Stack<Tile> tileStack = new Stack<Tile>();
			tileStack.Push(tileToFill);

			while (tileStack.Count != 0)
			{
				GraphicTile currentTile = tileStack.Pop() as GraphicTile;

				if (currentTile.Graphic == graphicToMatch)
				{
					currentTile.Graphic = sourceTile.Graphic;

					if (currentTile.X > 0)
						tileStack.Push(m_openMap.GetTile(currentTile.X - 1, currentTile.Y, LAYERS.Graphic));

					if (currentTile.Y > 0)
						tileStack.Push(m_openMap.GetTile(currentTile.X, currentTile.Y - 1, LAYERS.Graphic));

					if (currentTile.X < m_openMap.Width - 1)
						tileStack.Push(m_openMap.GetTile(currentTile.X + 1, currentTile.Y, LAYERS.Graphic));

					if (currentTile.Y < m_openMap.Height - 1)
						tileStack.Push(m_openMap.GetTile(currentTile.X, currentTile.Y + 1, LAYERS.Graphic));
				}
			}
		}

		private void FillTileSpecial(Tile tileToFill, Tile fillTile)
		{
			SpecialTile destTile = tileToFill as SpecialTile, sourceTile = fillTile as SpecialTile;

			if (destTile.Type == sourceTile.Type)
				return;

			SpecialTileSpec graphicToMatch = destTile.Type;

			Stack<Tile> tileStack = new Stack<Tile>();
			tileStack.Push(tileToFill);

			while (tileStack.Count != 0)
			{
				SpecialTile currentTile = tileStack.Pop() as SpecialTile;

				if (currentTile.Type == graphicToMatch)
				{
					currentTile.CopyTypeFrom(sourceTile);

					if (currentTile.X > 0)
						tileStack.Push(m_openMap.GetTile(currentTile.X - 1, currentTile.Y, LAYERS.Special));

					if (currentTile.Y > 0)
						tileStack.Push(m_openMap.GetTile(currentTile.X, currentTile.Y - 1, LAYERS.Special));

					if (currentTile.X < m_openMap.Width - 1)
						tileStack.Push(m_openMap.GetTile(currentTile.X + 1, currentTile.Y, LAYERS.Special));

					if (currentTile.Y < m_openMap.Height - 1)
						tileStack.Push(m_openMap.GetTile(currentTile.X, currentTile.Y + 1, LAYERS.Special));
				}
			}
		}

		private void UpdateMapViewer_MouseWheel(object sender, MouseEventArgs e)
		{
			if (ModifierKeys.HasFlag(Keys.Control))
			{
				if (e.Delta > 0)
					mnuZoomIn_Click(null, null);
				else if (e.Delta < 0)
					mnuZoomOut_Click(null, null);
			}
			else
			{
				if (ModifierKeys.HasFlag(Keys.Shift))
					mapViewer1.Offset = new Vector2(mapViewer1.Offset.X + ((float)e.Delta / 50f), mapViewer1.Offset.Y);
				else
					mapViewer1.Offset = new Vector2(mapViewer1.Offset.X, mapViewer1.Offset.Y + ((float)e.Delta / 50f));

				mapViewer1.Refresh();
			}
		}

		private void UpdateMapViewer_MouseMove(object sender, MouseEventArgs e)
		{
			if (!m_fileOpen)
				return;
			lock (lockObject)
			{
				//update logic below
				int x, y;
				int zf = (int)(mapViewer1.TileSize * mapViewer1.ZoomFactor);

				x = e.X - (e.X % zf) + ((int)mapViewer1.Offset.X % zf);
				y = e.Y - (e.Y % zf) + ((int)mapViewer1.Offset.Y % zf);

				if (x < mapViewer1.Offset.X)
					x = (int)mapViewer1.Offset.X;
				else if (x > mapViewer1.Area.Width + mapViewer1.Offset.X - zf)
					x = mapViewer1.Area.Width + (int)mapViewer1.Offset.X - zf;

				if (y < mapViewer1.Offset.Y)
					y = (int)mapViewer1.Offset.Y;
				else if (y > mapViewer1.Area.Height + mapViewer1.Offset.Y - zf)
					y = mapViewer1.Area.Height + (int)mapViewer1.Offset.Y - zf;
				
				mapViewer1.SelectorPosition = new Vector2((float)x, (float)y);
				x = (int)(x - mapViewer1.Offset.X) / zf;
				y = (int)(y - mapViewer1.Offset.Y) / zf;

				if(shiftKeyPressed && prevShift && e.Button == MouseButtons.Left)
				{
					if(dragdir == DragState.NoDrag)
					{
						if (y == orig_y && x != orig_x)
							dragdir = DragState.Horizontal; //horizontal
						else if (x == orig_x && y != orig_y)
							dragdir = DragState.Vertical; //vertical
					}

					switch(dragdir)
					{
						//force coordinates to be what the originals were
						case DragState.Horizontal: y = orig_y; break;
						case DragState.Vertical: x = orig_x; break;
					}
				}

				tsLblSelX.Text = x.ToString();
				tsLblSelY.Text = y.ToString();

				switch (currentState)
				{
					case PlaceState.SINGLEFILL:
						if (e.Button == System.Windows.Forms.MouseButtons.Left)
						{
							Tile toAdd;
							switch (tabs.SelectedIndex)
							{
								case (int)LAYERS.Graphic:
									if (lvTiles.SelectedIndices.Count == 0)
										break;

									//if (tileset.Tiles[lvTiles.SelectedIndices[0]] is AnimatedTile)
									//{
									//	int index = tileset.CorrectedTileIndex(lvTiles.SelectedIndices[0], TileType.Animated);
									//	toAdd = new AnimatedTile(x, y, index);
									//}
									//else
									//{
										toAdd = new GraphicTile(x, y);
										(toAdd as GraphicTile).Graphic = tileset.CorrectedTileIndex<GraphicTile>(lvTiles.SelectedIndices[0]);
									//}
									m_openMap.AddTile(x, y, LAYERS.Graphic, toAdd);
									break;
								case (int)LAYERS.Special:
									float density = 0.0f;
									if (!CreateSpecialTile(out toAdd)
										|| ((toAdd as SpecialTile).Type == SpecialTileSpec.WALL && !float.TryParse(txtDensity.Text, out density)))
										break;

									(toAdd as SpecialTile).Type = (SpecialTileSpec)cmbSpecialType.SelectedIndex;

									if ((toAdd as SpecialTile).Type == SpecialTileSpec.WALL)
										(toAdd as SpecialTile).Density = density;

									(toAdd as SpecialTile).Graphic = tileset.CorrectedTileIndex<SpecialTile>(lvTiles2.SelectedIndices[0]);

									m_openMap.AddTile(toAdd.X, toAdd.Y, LAYERS.Special, toAdd);
									break;
								case (int)LAYERS.NPC:
									//first check if there is an NPC spawn. if there is, open up a dialog to edit it
									//  otherwise, create a new spawn
									int ID; uint speed = 0;
									if (!int.TryParse(txtNPCId.Text, out ID) || (chkNPCMoves.Checked && !uint.TryParse(txtNPCMoveSpeed.Text, out speed)))
									{
										MessageBox.Show("Please enter valid NPC id (integer)");
										break;
									}
									if (chkNPCMoves.Checked)
										toAdd = new NPCTile(x, y, ID, speed);
									else
										toAdd = new NPCTile(x, y, ID);
									m_openMap.AddTile(x, y, LAYERS.NPC, toAdd);
									break;
								case (int)LAYERS.Interactive: //using this as item layer for now
									//first check if there is an Item spawn. if there is, open up a dialog to edit it
									//  otherwise, create a new spawn
									break;
							}
						}
						break;
					case PlaceState.MULTIFILL:
						if (e.Button == System.Windows.Forms.MouseButtons.Left)
						{ //the previous mouse state recorded the left button clicked
							if (tabs.SelectedIndex == (int)LAYERS.Special)
							{
								Tile based = m_openMap.GetTile(x, y, LAYERS.Special), spec;
								if (based != null)
									break;
								if (!CreateSpecialTile(out spec))
									break;

								FillTileSpecial(based, spec);
							}
							else if (tabs.SelectedIndex == (int)LAYERS.Graphic)
							{
								Tile toAdd, current = m_openMap.GetTile(x, y, LAYERS.Graphic);

								if (lvTiles.SelectedItems.Count == 0)
									return;

								if (tileset.Tiles[tileset.CorrectedTileIndex<AnimatedTile>(lvTiles.SelectedIndices[0])] is AnimatedTile)
								{
									int index = tileset.CorrectedTileIndex<AnimatedTile>(lvTiles.SelectedIndices[0]);
									toAdd = new AnimatedTile(x, y, index);
								}
								else
								{
									toAdd = new GraphicTile(x, y);
									(toAdd as GraphicTile).Graphic = tileset.CorrectedTileIndex<GraphicTile>(lvTiles.SelectedIndices[0]);
								}

								FillTileGraphic(current, toAdd);
							}

							m_buffer.AddState(m_openMap);
							UpdateUndoRedo();
						}
						break;
					case PlaceState.ERASER:
						if (e.Button == MouseButtons.Left)
						{
							m_openMap.EraseTile(int.Parse(tsLblSelX.Text), int.Parse(tsLblSelY.Text), (LAYERS)tabs.SelectedIndex);
						}
						else if (prevMousePressed && e.Button != MouseButtons.Left)
						{
							m_buffer.AddState(m_openMap);
							UpdateUndoRedo();
						}
						break;
					case PlaceState.PAN:
						if (e.Button == System.Windows.Forms.MouseButtons.Left)
						{
							using (MemoryStream mem = new MemoryStream(Properties.Resources.hand_closed))
							{
								mapViewer1.Cursor = new Cursor(mem);
							}
							if (!prevMousePressed)
							{
								originalLocation.X = e.X;
								originalLocation.Y = e.Y;
								originalAdjustment.X = mapViewer1.Offset.X;
								originalAdjustment.Y = mapViewer1.Offset.Y;
							}
							else
							{
								mapViewer1.Offset = new Vector2(originalAdjustment.X + (e.X - originalLocation.X), originalAdjustment.Y + (e.Y - originalLocation.Y));
							}
						}
						else
						{
							using (MemoryStream mem = new MemoryStream(Properties.Resources.hand_cur))
							{
								mapViewer1.Cursor = new Cursor(mem);
							}
						}
						break;
				}

				prevMousePressed = (e.Button == System.Windows.Forms.MouseButtons.Left);
				if (shiftKeyPressed && !prevShift && prevMousePressed)
				{
					dragdir = DragState.NoDrag;
					prevShift = true;
					orig_x = x;
					orig_y = y;
				}

				mapViewer1.State = currentState;
				mapViewer1.ShowGrid = mnuGrid.Checked;
				tsLblNumTiles.Text = "";
				foreach (SortedList<string, Tile> sublist in m_openMap.Layers)
				{ //show the number of tiles in each layer in the appropriate TS label
					tsLblNumTiles.Text += sublist.Count.ToString();
					if (sublist != m_openMap.Layers.Last())
						tsLblNumTiles.Text += " / ";
				}

				mapViewer1.Refresh(); //force the control to redraw after updating
			}
		}

		private void mnuNew_Click(object sender, EventArgs e)
		{
			//create a new file in the editor
			if (m_fileOpen)
				if (!PromptSaveChangesAndClose())
					return;

			using (frmNewMap newMap = new frmNewMap())
			{
				newMap.ShowDialog();
				if (newMap.mName == null)
					return;

				m_openMap = new Map(newMap.mName, newMap.mWidth, newMap.mHeight);
				m_fileOpen = true;
			}

			OpenEnableControls();
			mapViewer1.SetMapData(m_openMap.Layers, m_openMap.Width, m_openMap.Height);
		}

		private void mnuOpen_Click(object sender, EventArgs e)
		{
			OpenFile();
		}

		private void OpenFile(string fName = null)
		{
			//open an existing file
			string filepath;
			if (fName == null)
			{
				using (OpenFileDialog ofd = new OpenFileDialog())
				{
					ofd.AddExtension = true;
					ofd.CheckFileExists = true;
					ofd.CheckPathExists = true;
					ofd.DefaultExt = "bmap";
					ofd.Filter = "Binary Map|*.bmap|Plaintext Map|*.txt";
					DialogResult dr = ofd.ShowDialog();
					if (dr == DialogResult.OK)
						filepath = ofd.FileName;
					else
						return;
				}
			}
			else
				filepath = fName;

			Map newMap = new Map(filepath);
			if (!newMap.Loaded)
			{
				MessageBox.Show("Error loading map!");
				return;
			}

			m_openMap = newMap;
			OpenEnableControls();
			mapViewer1.SetMapData(m_openMap.Layers, m_openMap.Width, m_openMap.Height);
			RegistryAccess.AddRecentFile(m_openMap.FileName, RegistrySetting.RecentMaps);
			RefreshRecentList(mnuFileRecent, RegistrySetting.RecentMaps);
		}

		private void mnuSave_Click(object sender, EventArgs e)
		{
			if(m_openMap.Saved)
				return;
			string filepath = m_openMap.FileName;
			if (string.IsNullOrEmpty(filepath))
				SaveAsHelper(false);
			else
			{
				bool plain = filepath.EndsWith(".ptxt");
				try
				{
					if (!m_openMap.Save(filepath, plain))
						MessageBox.Show("Error saving file!");
					else
					{
						m_buffer.UpdateFilePath(filepath);
						tslblFileName.Text = filepath;
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void mnuSaveAs_Click(object sender, EventArgs e)
		{
			SaveAsHelper(true);
		}

		private void mnuClose_Click(object sender, EventArgs e)
		{
			if (m_fileOpen && !m_openMap.Saved && !PromptSaveChangesAndClose())
				return;

			CloseDisableControls();
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (m_fileOpen && !m_openMap.Saved && !PromptSaveChangesAndClose())
			{
				e.Cancel = true;
				return;
			}
		}

		private void tsNew_Click(object sender, EventArgs e)
		{
			mnuNew_Click(sender, e);
		}

		private void tsOpen_Click(object sender, EventArgs e)
		{
			mnuOpen_Click(sender, e);
		}

		private void tsSave_Click(object sender, EventArgs e)
		{
			mnuSave_Click(sender, e);
		}

		private void tsArrow_Click(object sender, EventArgs e)
		{
			tsErase.Checked = false;
			tsPan.Checked = false;
			tsMArrow.Checked = false;
			currentState = PlaceState.SINGLEFILL;

			mapViewer1.Cursor = Cursors.Default;
		}

		private void tsErase_Click(object sender, EventArgs e)
		{
			tsArrow.Checked = false;
			tsPan.Checked = false;
			tsMArrow.Checked = false;
			currentState = PlaceState.ERASER;

			mapViewer1.Cursor = Cursors.Default;
		}

		private void tsPan_Click(object sender, EventArgs e)
		{
			tsArrow.Checked = false;
			tsErase.Checked = false;
			tsMArrow.Checked = false;
			currentState = PlaceState.PAN;

			MemoryStream str = new MemoryStream(Properties.Resources.hand_cur);
			mapViewer1.Cursor = new Cursor(str);
			str.Close();
			tsLblSelX.Text = "";
			tsLblSelY.Text = "";
		}

		private void tsMArrow_Click(object sender, EventArgs e)
		{
			tsMArrow.Checked = true;
			tsArrow.Checked = false;
			tsErase.Checked = false;
			tsPan.Checked = false;
			currentState = PlaceState.MULTIFILL;

			mapViewer1.Cursor = Cursors.Default;
		}

		private void lvTiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lvTiles.SelectedIndices.Count == 0)
				pbTilePreview.Image = new Bitmap(1, 1);
			else
				pbTilePreview.Image = tileset.Images.Images[tileset.CorrectedTileIndex<GraphicTile>(lvTiles.SelectedIndices[0])];
		}

		private void mnuGrid_Click(object sender, EventArgs e)
		{
			mapViewer1.ShowGrid = mnuGrid.Checked;
			mapViewer1.Refresh();
		}

		private void mnuTileLayer_Click(object sender, EventArgs e)
		{
			mapViewer1.LayerTile = mnuTileLayer.Checked;
			mapViewer1.Refresh();
		}

		private void mnuSpecialLayer_Click(object sender, EventArgs e)
		{
			mapViewer1.LayerSpecial = mnuSpecialLayer.Checked;
			mapViewer1.Refresh();
		}

		private void mnuFill_Click(object sender, EventArgs e)
		{
			if(lvTiles.SelectedIndices.Count == 0)
				return;

			bool animMode = tileset.Tiles[lvTiles.SelectedIndices[0]] is AnimatedTile;

			for (int i = 0; i < m_openMap.Height; ++i)
			{
				for (int j = 0; j < m_openMap.Width; ++j)
				{
					GraphicTile newTile = animMode ? new AnimatedTile(j, i, lvTiles.SelectedIndices[0]) : new GraphicTile(j, i, lvTiles.SelectedIndices[0]);
					m_openMap.AddTile(j, i, LAYERS.Graphic, newTile);
				}
			}

			m_buffer.AddState(m_openMap);
			UpdateUndoRedo();

			mapViewer1.Refresh();
		}

		private void mnuClearLayer_Click(object sender, EventArgs e)
		{
			LAYERS layer = (LAYERS)tabs.SelectedIndex;

			if (layer != LAYERS.Graphic && layer != LAYERS.Special)
				return;

			if (m_openMap.Layers[(int)layer].Count == 0)
				return;

			DialogResult res = MessageBox.Show("This will clear all tile data on the selected layer. Continue?", "Continue", MessageBoxButtons.YesNo);
			if (res == DialogResult.Yes)
			{
				m_openMap.ClearLayer(layer);

				m_buffer.AddState(m_openMap);
				UpdateUndoRedo();

				mapViewer1.Refresh();
			}
		}

		private void mnuUndo_Click(object sender, EventArgs e)
		{
			m_openMap = m_buffer.GetUndo() ?? m_openMap;

			mapViewer1.SetMapData(m_openMap.Layers, m_openMap.Width, m_openMap.Height);
			mapViewer1.Refresh();
			mnuUndo.Enabled = m_buffer.UndoReady;
			mnuRedo.Enabled = m_buffer.RedoReady;
		}

		private void mnuRedo_Click(object sender, EventArgs e)
		{
			m_openMap = m_buffer.GetRedo() ?? m_openMap;

			mapViewer1.SetMapData(m_openMap.Layers, m_openMap.Width, m_openMap.Height);
			mapViewer1.Refresh();
			mnuUndo.Enabled = m_buffer.UndoReady;
			mnuRedo.Enabled = m_buffer.RedoReady;
		}

		private void hmnuEraser_Click(object sender, EventArgs e)
		{
			tsErase_Click(sender, e);
			tsErase.Checked = true;
		}

		private void hmnuPan_Click(object sender, EventArgs e)
		{
			tsPan_Click(sender, e);
			tsPan.Checked = true;
		}

		private void mnuTestMap_Click(object sender, EventArgs e)
		{
			//update PokemonGame with render logic once completed with the map editor
			//this will be completed last
			Thread testThread = new Thread(new ParameterizedThreadStart((object o) =>
			{
				//create copies of those objects on this thread
				Map map = new Map(o as Map);
				//PokemonGame pg = new PokemonGame(map);
				//pg.Run();

				throw new NotImplementedException();
			}));

			testThread.Start(m_openMap);
			testThread.Join();
		}

		private void mnuZoomIn_Click(object sender, EventArgs e)
		{
			if (zoomIndex == zoomLevels.Length - 1)
				return;
			mapViewer1.ZoomFactor = zoomLevels[++zoomIndex];

			tslblZoomLevel.Text = (100 * zoomLevels[zoomIndex]).ToString() + "%";
			mapViewer1.Refresh();
		}

		private void mnuZoomOut_Click(object sender, EventArgs e)
		{
			if (zoomIndex == 0)
				return;
			mapViewer1.ZoomFactor = zoomLevels[--zoomIndex];

			tslblZoomLevel.Text = (100 * zoomLevels[zoomIndex]).ToString() + "%";
			mapViewer1.Refresh();
		}

		private void mnuProperties_Click(object sender, EventArgs e)
		{
			using (frmMapProperties props = new frmMapProperties(this.m_openMap))
			{
				DialogResult dr = props.ShowDialog();
				if (dr == DialogResult.Cancel)
					return;

				this.m_openMap = new Map(props.Map);
				this.m_buffer.AddState(m_openMap);
				UpdateUndoRedo();

				mapViewer1.SetMapData(m_openMap.Layers, m_openMap.Width, m_openMap.Height);
				mapViewer1.Refresh();
			}
		}

		private void mnuSpawnData_Click(object sender, EventArgs e)
		{
			using (frmSpawnEditor se = new frmSpawnEditor(m_openMap.Spawns))
			{
				se.ShowDialog();
				if (se.SpawnsChanged)
				{
					m_openMap.SetSpawnData(se.Spawns);
					m_buffer.AddState(m_openMap);
				}
			}
		}

		private void cmbSpecialType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_fileOpen)
			{
				grpWarpPlacement.Enabled = false;
				return;
			}

			if (cmbSpecialType.SelectedItem == null)
				return;

			SpecialTileSpec t = (SpecialTileSpec)Enum.Parse(typeof(SpecialTileSpec), cmbSpecialType.SelectedItem.ToString());
			switch (t)
			{
				case SpecialTileSpec.WARP:
					grpWarpPlacement.Enabled = true;
					break;
				default:
					grpWarpPlacement.Enabled = false;
					break;
			}
		}

		private void frmMain_KeyDown(object sender, KeyEventArgs e)
		{
			if (!m_fileOpen)
				return;
			if (e.KeyCode == Keys.ShiftKey)
			{
				shiftKeyPressed = true;
			}
		}

		private void frmMain_KeyUp(object sender, KeyEventArgs e)
		{ //reset animated tiles to their original state (sync them up) when F12 is pressed
			if (!m_fileOpen)
				return;
			if (e.KeyCode == System.Windows.Forms.Keys.F12)
				mapViewer1.ResetAnim();
			else if(e.KeyCode == Keys.ShiftKey)
			{
				shiftKeyPressed = false;
				prevShift = false;
				dragdir = DragState.NoDrag;
			}
		}

		private void mnuAbout_Click(object sender, EventArgs e)
		{
			using (frmAbout ab = new frmAbout())
				ab.ShowDialog();
		}

		private void recentFile_click(object sender, EventArgs e)
		{
			OpenFile((sender as ToolStripMenuItem).Tag as string);
		}

		private void RefreshRecentList(ToolStripMenuItem menuItem, RegistrySetting setting)
		{
			string[] fileNames = (string[])RegistryAccess.GetValueOrDefault(setting, new string[] {""}, Microsoft.Win32.RegistryValueKind.MultiString);

			if (fileNames.Length == 1 && fileNames[0] == "")
				return;

			if (menuItem.HasDropDownItems)
				menuItem.DropDownItems.Clear();

			for (int i = 0; i < fileNames.Length; ++i)
			{
				menuItem.DropDownItems.Add((i + 1) + "  " + RegistryAccess.MakeShorter(fileNames[i]), null, new EventHandler(recentFile_click));
				menuItem.DropDownItems[i].Tag = fileNames[i];
			}

			if (menuItem.HasDropDownItems)
				menuItem.Enabled = true;
			else
				menuItem.Enabled = false;
		}

		private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditTileset();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sd = new SaveFileDialog();
			sd.Title = "Save tileset file";
			sd.Filter = "Tileset files|*.tileset";
			sd.ShowDialog();

			tileset.SaveToFile(sd.FileName);
			RegistryAccess.AddRecentFile(tileset.FileName, RegistrySetting.RecentTilesets);
			RefreshRecentList(recentToolStripMenuItem, RegistrySetting.RecentTilesets);
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog od = new OpenFileDialog();
			od.Title = "Open tileset file";
			od.Filter = "Tileset files|*.tileset";
			od.ShowDialog();

			Tileset temp = new Tileset();
			temp.LoadFromFile(od.FileName);
			mapViewer1.TileSize = temp.Images.ImageSize.Width;

			if (!temp.CheckAgainst(m_openMap))
			{
				if (MessageBox.Show("Some features of the current map don't work with this tileset.\n Would you like to close the current map?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
					mnuClose_Click(null, null);

				if (m_fileOpen)
					return;
			}

			tileset = temp;

			RefreshListViewer();
			RegistryAccess.AddRecentFile(tileset.FileName, RegistrySetting.RecentTilesets);
			RefreshRecentList(recentToolStripMenuItem, RegistrySetting.RecentTilesets);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (tileset.FileName == "")
			{
				saveAsToolStripMenuItem_Click(null, null);
				return;
			}
			else
				tileset.Save();
		}

		private void txtDensity_TextChanged(object sender, EventArgs e)
		{
			if (txtDensity.Text == "")
				txtDensity.Text = "0";
		}
	}
}
