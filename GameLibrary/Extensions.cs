using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using XNAFramework = Microsoft.Xna.Framework;
using XNAControls;

namespace GameClassLibrary
{
	public static class Extensions
	{

		// Since C# doesn't know how to convert ints to bool. Like an IDIOT
		public static bool ToBool(this int value)
		{
			return value != 0;
		}

		public static XNAFramework.Rectangle PositionOnVisibleMap(this Map map, int x, int y)
		{
			return new XNAFramework.Rectangle(
				(int)map.UpperLeftCorner.X + (x - map.VisibleBounds.Left) * Const.TILE_SIZE,
				(int)map.UpperLeftCorner.Y + (y - map.VisibleBounds.Top) * Const.TILE_SIZE,
				Const.TILE_SIZE,
				Const.TILE_SIZE);
		}

		public static bool ContainsAll(this XNAFramework.GameComponentCollection collection, XNAFramework.GameComponentCollection other)
		{
			foreach (XNAFramework.GameComponent component in other)
			{
				if (!collection.Contains(component))
					return false;
			}

			return true;
		}

		public static void ArraySwap<T>(T[] data, int a, int b)
		{
			T temp = data[a];
			data[a] = data[b];
			data[b] = temp;
		}

		public static void RefreshContents(this System.Windows.Forms.ListView listView, Tileset tileSet, TileType? type = null)
		{
			listView.Items.Clear();

			for (int j = 0; j < tileSet.Images.Images.Count; ++j)
			{
				switch (type)
				{
					case null:
						listView.Items.Add("", j);
						break;
					case TileType.Animated:
					case TileType.Graphic:
						if (!(tileSet.Tiles[j] is SpecialTile))
							listView.Items.Add("", j);
						break;
					case TileType.Special:
						if (tileSet.Tiles[j] is SpecialTile)
							listView.Items.Add("", j);
						break;
				}
			}

			listView.LargeImageList = tileSet.Images;

			if (listView.Items.Count > 0)
			{
				listView.SelectedIndices.Clear();
				listView.SelectedIndices.Add(0);
			}
		}

		#region Stream functions

		public static void WriteInt(this Stream stream, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			for (int i = 0; i < bytes.Length; i++)
				stream.WriteByte(bytes[i]);
		}

		public static int ReadInt(this Stream stream)
		{
			byte[] bytes = new byte[4];

			for (int i = 0; i < 4; i++)
				bytes[i] = stream.ReadByteAsByte();

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			return BitConverter.ToInt32(bytes, 0);
		}

		public static void WriteDouble(this Stream stream, double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			for (int i = 0; i < bytes.Length; i++)
				stream.WriteByte(bytes[i]);
		}

		public static double ReadDouble(this Stream stream)
		{
			byte[] bytes = new byte[8];

			for (int i = 0; i < 8; i++)
				bytes[i] = stream.ReadByteAsByte();

			if (BitConverter.IsLittleEndian)
				Array.Reverse(bytes);

			return BitConverter.ToDouble(bytes, 0);
		}

		public static byte ReadByteAsByte(this Stream stream)
		{
			return (byte)stream.ReadByte();
		}

		public static char ReadChar(this Stream stream)
		{
			byte[] bytes = new byte[2];
			bytes[0] = stream.ReadByteAsByte();
			bytes[1] = stream.ReadByteAsByte();

			return BitConverter.ToChar(bytes, 0);
		}

		public static void WriteChar(this Stream stream, char value)
		{
			byte[] bytes = BitConverter.GetBytes(value);

			for (int i = 0; i < bytes.Length; i++)
				stream.WriteByte(bytes[i]);
		}

		public static string ReadString(this Stream stream)
		{
			int size = stream.ReadInt();

			string result = "";

			for (int i = 0; i < size; i++)
				result += stream.ReadChar();

			return result;
		}

		public static void WriteString(this Stream stream, string value)
		{
			stream.WriteInt(value.Length);

			for (int i = 0; i < value.Length; i++)
				stream.WriteChar(value[i]);
		}

		#endregion

		#region Drawing

		public static Texture2D DrawRadialGradient(this XNAFramework.Game game, int radius, Color insideColor, Color outsideColor, Color? surroundingColor = null)
		{
			if (surroundingColor == null)
				surroundingColor = outsideColor;

			Rectangle bounds = new Rectangle(0, 0, radius * 2, radius * 2);
			using (var ellipsePath = new GraphicsPath())
			{
				ellipsePath.AddEllipse(bounds);
				using (var brush = new PathGradientBrush(ellipsePath))
				{
					// Set up radial gradient brush
					brush.CenterPoint = new PointF(bounds.Width / 2f, bounds.Height / 2f);
					brush.CenterColor = insideColor;
					brush.SurroundColors = new[] { outsideColor };
					brush.FocusScales = new PointF(0, 0);

					Bitmap bm = new Bitmap(radius * 2, radius * 2);
					Graphics graphicsObject = Graphics.FromImage(bm);

					// Set up elliptical clip
					GraphicsPath clipPath = new GraphicsPath();
					clipPath.AddEllipse(bounds);

					// Fill with outsideColor outside of the clip
					graphicsObject.ExcludeClip(new Region(clipPath));
					graphicsObject.FillRectangle(new SolidBrush(surroundingColor.Value), bounds);

					// Fill with gradient from outsideColor to insideColor inside of the clip
					graphicsObject.SetClip(clipPath);
					graphicsObject.FillRectangle(brush, bounds);

					return bm.ToTexture2D(game);
				}
			}
		}


		#endregion
	}
}
