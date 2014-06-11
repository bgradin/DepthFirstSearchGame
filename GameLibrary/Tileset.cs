using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GameClassLibrary
{
	public class Tileset
	{
		public string Name { get; set; }
		public string FileName { get; set; }
		public bool FileUpToDate { get; set; }
		public ImageList Images { get; set; }
		public List<Tile> Tiles { get; private set; }

		public Tileset(string filename = "")
		{
			Tiles = new List<Tile>();
			Images = new ImageList();
			Name = "tileset";
			FileName = filename;
			FileUpToDate = false;
		}

		public Tileset(Tileset ts)
		{
			Tiles = new List<Tile>(ts.Tiles);
			Images = new ImageList();
			Images.ColorDepth = ts.Images.ColorDepth;
			Images.ImageSize = new Size(ts.Images.ImageSize.Width, ts.Images.ImageSize.Height);
			Name = ts.Name;
			FileName = ts.FileName;
			FileUpToDate = ts.FileUpToDate;

			foreach (Image image in ts.Images.Images)
				Images.Images.Add((Image)image.Clone());
		}

		public bool CheckAgainst(Map map)
		{
			if (map == null)
				return true;

			foreach (Tile tile in map.Layers[(int)LAYERS.Graphic].Values)
			{
				if (tile is GraphicTile)
				{
					if ((tile as GraphicTile).Graphic > Images.Images.Count)
						return false;
				}
			}

			return true;
		}

		public bool LoadFromStream(Stream stream)
		{
			int magicNumber = stream.ReadInt(); // Make sure we have a valid tileset file

			if (magicNumber != Const.MAGIC_NUMBER)
				throw new Exception();

			string name = stream.ReadString();

			ImageList imageList = new ImageList();
			int size = stream.ReadInt();
			imageList.ImageSize = new Size(size, size);
			imageList.ColorDepth = (ColorDepth)stream.ReadByte();

			List<Tile> tiles = new List<Tile>();

			while (stream.Position < stream.Length)
			{
				int bla;
				switch ((Field)(bla = stream.ReadByte()))
				{
					case Field.Image:
						int sizeOfField = stream.ReadInt();

						byte[] imageBytes = new byte[sizeOfField - 2];
						for (int i = 0; i < imageBytes.Length; i++)
							imageBytes[i] = stream.ReadByteAsByte();

						using (var ms = new MemoryStream(imageBytes))
						{
							imageList.Images.Add(Image.FromStream(ms));
						}
						break;

					case Field.GraphicTile:
						GraphicTile gt = new GraphicTile(-1);
						gt.Load(stream);
						tiles.Add(gt);
						break;

					case Field.SpecialTile:
						SpecialTile st = new SpecialTile(-1, -1);
						st.Load(stream);
						tiles.Add(st);
						break;
				}
			}

			Name = name;
			FileName = "";
			FileUpToDate = true;
			Images = imageList;
			Tiles = tiles;

			return true;
		}

		public bool LoadFromFile(string filename)
		{
			try
			{
				using (StreamReader reader = new StreamReader(filename))
				{
					if (!LoadFromStream(reader.BaseStream))
						return false;

					FileName = filename;
					return true;
				}
			}
			catch
			{
				MessageBox.Show("There was a problem loading the specified tileset.");
				return false;
			}
		}

		public bool Save()
		{
			if (!FileUpToDate && SaveToFile(FileName))
				FileUpToDate = true;

			return FileUpToDate;
		}

		public bool SaveToFile(string filename)
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(filename))
				{
					Stream stream = writer.BaseStream;

					stream.WriteInt(Const.MAGIC_NUMBER); // Write magic number to identify this as a valid tileset
					stream.WriteString(Name); // Write the tileset name
					stream.WriteInt(Images.ImageSize.Width); // Write size of images
					stream.WriteByte((byte)ColorDepth.Depth24Bit); // Write color depth

					for (int i = 0; i < Images.Images.Count; i++)
					{
						using (MemoryStream ms = new MemoryStream())
						{
							stream.WriteByte((byte)Field.Image); // Field identifier

							Images.Images[i].Save(ms, System.Drawing.Imaging.ImageFormat.Png);
							byte[] bytes = ms.ToArray();

							stream.WriteInt(bytes.Length + 2); // Length of field

							for (int j = 0; j < bytes.Length; j++)
								stream.WriteByte(bytes[j]);
						}
					}

					for (int i = 0; i < Tiles.Count; i++)
						Tiles[i].Save(stream);
				}

				FileName = filename;
			}
			catch
			{
				MessageBox.Show("There was a problem saving the file.");
				return false;
			}

			return true;
		}
	}
}
