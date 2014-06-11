using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameClassLibrary
{
	public abstract class Tile
	{
		protected int x_coord;
		protected int y_coord;

		public Tile()
		{
			x_coord = -1;
			y_coord = -1;
		}

		public Tile(int x, int y)
		{
			x_coord = x;
			y_coord = y;
		}

		public int X { get { return this.x_coord; } }
		public int Y { get { return this.y_coord; } }

		public virtual void Save(Stream stream)
		{
			stream.WriteByte((byte)Field.Tile);
			stream.WriteInt(x_coord);
			stream.WriteInt(y_coord);
		}

		public virtual void Load(Stream stream)
		{
			x_coord = stream.ReadInt();
			y_coord = stream.ReadInt();
		}
	}

	public class GraphicTile : Tile
	{
		public GraphicTile(int index)
		{
			Graphic = index;
		}

		public GraphicTile(int x, int y)
			: base(x, y)
		{
			Graphic = 0;
		}

		public GraphicTile(int x, int y, int graphicNumber)
			: base(x, y)
		{
			Graphic = graphicNumber;
		}

		public int Graphic { get; set; }

		public override void Save(Stream stream)
		{
			stream.WriteByte((byte)Field.GraphicTile);
			stream.WriteInt(Graphic);
			base.Save(stream);
		}

		public override void Load(Stream stream)
		{
			Graphic = stream.ReadInt();
			stream.ReadByte(); // Throwaway byte (Field.Tile was written here)
			base.Load(stream);
		}
	}

	public class AnimatedTile : GraphicTile
	{
		public AnimatedTile(int x, int y) : base(x, y) { }
		public AnimatedTile(int x, int y, int graphic) : base(x, y, graphic) { Graphic = graphic; frame = 0; }

		public void Animate()
		{
			frame++;
			if (frame >= 4) //four animation states
				frame = 0;
		}

		public void Reset()
		{
			frame = 0;
		}

		private int frame;
		public int Frame { get { return this.frame; } }

		public override void Save(Stream stream)
		{
			throw new NotImplementedException();
		}
		public override void Load(Stream stream)
		{
			throw new NotImplementedException();
		}
	}

	public class SpecialTile : GraphicTile
	{
		private int d_map, d_x, d_y; //dest map warpid, x, y for warps
		private WarpAnim d_anim;
		private float density = 0.0f;

		public SpecialTileSpec Type { get; set; }

		public float Density
		{
			get
			{
				if (Type != SpecialTileSpec.WALL)
					throw new InvalidOperationException("This type of SpecialTile is not a wall.");
				return density;
			}
			set
			{
				density = value;
			}
		}

		public int WarpMap
		{
			get
			{
				if (Type != SpecialTileSpec.WARP)
					throw new InvalidOperationException("This type of SpecialTile is not a warp.");
				return d_map;
			}
		}

		public int WarpX
		{
			get
			{
				if (Type != SpecialTileSpec.WARP)
					throw new InvalidOperationException("This type of SpecialTile is not a warp.");
				return d_x;
			}
		}

		public int WarpY
		{
			get
			{
				if (Type != SpecialTileSpec.WARP)
					throw new InvalidOperationException("This type of SpecialTile is not a warp.");
				return d_y;
			}
		}

		public WarpAnim WarpAnim
		{
			get
			{
				if (Type != SpecialTileSpec.WARP)
					throw new InvalidOperationException("This type of SpecialTile is not a warp.");
				return this.d_anim;
			}
		}

		public SpecialTile(int x, int y)
			: base(x, y)
		{
			Type = SpecialTileSpec.NONE;
		}

		/// <summary>
		/// Format - None/Wall/Jump: null; Grass/Water: spawn_id; Warp: dest_warp_id, dest_x, dest_y, warpanim;
		/// </summary>
		/// <param name="type">Type to set the special tyle as</param>
		/// <param name="list">See format in summary</param>
		public void SetType(SpecialTileSpec type, params object[] list)
		{
			//updates stored values when type == Type
			switch (type)
			{
				case SpecialTileSpec.WALL:
					if (list != null)
						throw new ArgumentException("Invalid parameters for this TileType");
					Type = type;
					d_map = d_x = d_y = -1;
					break;
				case SpecialTileSpec.WARP: //TODO: add in WarpAnim
					throw new NotImplementedException();
					if (list == null || list.Length != 4
						|| list[0].GetType() != typeof(int) || list[1].GetType() != typeof(int)
						|| list[2].GetType() != typeof(int) || list[3].GetType() != typeof(WarpAnim))
						throw new ArgumentException("Invalid parameters for this TileType");
					Type = type;
					d_map = (int)list[0];
					d_x = (int)list[1];
					d_y = (int)list[2];
					d_anim = (WarpAnim)list[3];
				case SpecialTileSpec.NONE:
					break;
				case SpecialTileSpec.NUM_VALS:
				default:
					throw new InvalidOperationException("TileType must be a valid value.");
			}
		}

		public void CopyTypeFrom(SpecialTile other)
		{
			this.Type = other.Type;
			this.d_map = other.d_map;
			this.d_x = other.d_x;
			this.d_y = other.d_y;
			this.d_anim = other.d_anim;
			this.density = other.density;
		}

		public override void Save(Stream stream)
		{
			stream.WriteByte((byte)Field.SpecialTile);
			stream.WriteByte((byte)Type);
			stream.WriteInt(d_map);
			stream.WriteInt(d_x);
			stream.WriteInt(d_y);
			stream.WriteByte((byte)d_anim);
			stream.WriteDouble(density);
			base.Save(stream);
		}

		public override void Load(Stream stream)
		{
			Type = (SpecialTileSpec)stream.ReadByte();
			d_map = stream.ReadInt();
			d_x = stream.ReadInt();
			d_y = stream.ReadInt();
			d_anim = (WarpAnim)stream.ReadByte();
			density = (float)stream.ReadDouble();
			stream.ReadByte(); // Throwaway byte (Field.GraphicTile was written here)
			base.Load(stream);
		}
	}

	public class ItemTile : Tile
	{
		static Random randomNumbers = new Random();

		public int Quantity { get; set; }

		public double Speed { get; private set; }

		public ItemTileSpec Type { get; private set; }

		public ItemTile(ItemTileSpec type, int x, int y, int quantity = 1)
			: base(x, y)
		{
			Type = type;
			Quantity = quantity;

			Speed = randomNumbers.NextDouble() * 0.15 + 0.072; // [num b/w 0 & 1] * range + minimum
		}
	}

	//TODO: Add interactive tile for interactive layer
	public class InteractiveTile : Tile
	{
		public InteractiveTile(int x, int y) : base(x, y) { }

		public override void Save(Stream stream)
		{
			throw new NotImplementedException();
		}

		public override void Load(Stream stream)
		{
			throw new NotImplementedException();
		}
	}

	//TODO: Add NPC tile for npcs moving around the map and shit
	// Each NPC tile should be a spawn for a moving NPC
	// Requires implementation of NPC class
	public class NPCTile : Tile
	{
		private bool doesMove = false;
		public bool Moves { get { return this.doesMove; } }

		private uint moveSpeedSec = 0;
		public uint MoveSpeed { get { return this.moveSpeedSec; } }

		private int id;
		public int NPCID { get { return this.id; } }

		/// <summary>
		/// Create a stationary NPC spawn
		/// </summary>
		/// <param name="x">x coordinate of spawn</param>
		/// <param name="y">y coordinate of spawn</param>
		/// <param name="NPC">NPC id</param>
		public NPCTile(int x, int y, int NPC)
			: base(x, y)
		{
			this.id = NPC;
		}
		/// <summary>
		/// Create a moving NPC spawn
		/// </summary>
		/// <param name="x">x coordinate of spawn</param>
		/// <param name="y">y coordinate of spawn</param>
		/// <param name="NPC">NPC id</param>
		/// <param name="speed">movement speed (seconds between moves)</param>
		public NPCTile(int x, int y, int NPC, uint speed)
			: base(x, y)
		{
			this.id = NPC;
			this.doesMove = true;
			this.moveSpeedSec = speed;
		}
		/// <summary>
		/// Change from moving to not moving
		/// </summary>
		/// <param name="moving">specify whether or not the NPC moves</param>
		/// <param name="speed">movement speed (seconds between moves)</param>
		public void Change(bool moving, uint speed = 0)
		{
			this.doesMove = moving;
			this.moveSpeedSec = speed;
		}

		public override void Save(Stream stream)
		{
			throw new NotImplementedException();
		}

		public override void Load(Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
