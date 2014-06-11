using GameClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerClassLibrary
{
	public static class World
	{
		public static LightsFX LightsFX { get; set; }

		public static GameState GameState { get; set; }

		public static List<Player> Players { get; set; }

		public static ConnectedPlayer MainPlayer
		{
			get
			{
				if (Players == null)
					return null;

				if (Players.Count > 0 && !(Players[0] is ConnectedPlayer))
					throw new InvalidOperationException("First player in the list is not a connected player");

				return Players.Count > 0 ? Players[0] as ConnectedPlayer : null;
			}
			set
			{
				if (Players == null)
					Players = new List<Player>();

				if (Players.Count > 0)
					Players[0] = value;
				else
					Players.Add(value);
			}
		}

		public static List<System.Drawing.Point> EmptyLocations(int index)
		{
			if (index >= Maps.Count)
				throw new IndexOutOfRangeException();

			List<System.Drawing.Point> result = Maps[index].GetEmptySpaces();

			if (Players != null)
				Players.ForEach(i => result.RemoveAll(j => j.Equals(i)));

			return result;
		}

		public static List<Map> Maps { get; set; }

		public static Map CurrentMap
		{
			get
			{
				return Maps.Count > 0 ? Maps[0] : null;
			}
			set
			{
				if (Maps == null)
					Maps = new List<Map>();

				if (Maps.Count > 0)
					Maps[0] = value;
				else
					Maps.Add(value);
			}
		}

		/// <summary>
		/// Does initial population of Bacteria for all maps in the world
		/// </summary>
		public static void PopulateBacteria()
		{
			Random gen = new Random();
			for (int i = 0; i < Maps.Count; i++)
			{
				List<System.Drawing.Point> emptySpaces = new List<System.Drawing.Point>(EmptyLocations(i));

				// Randomize emptySpaces using Fisher-Yates shuffling algorithm
				for (int j = 0; j < emptySpaces.Count; j++)
				{
					int rnd = gen.Next(0, emptySpaces.Count - 1);
					System.Drawing.Point tmp = emptySpaces[j];
					emptySpaces[j] = emptySpaces[rnd];
					emptySpaces[rnd] = tmp;
				}

				// Generate items at the first [size * proportion] locations
				int items = (int)Math.Floor(emptySpaces.Count * Const.FILL_PROPORTION);
				for (int j = 0; j < items; j++)
				{
					ItemTile tile = new ItemTile(ItemTileSpec.BACTERIA, emptySpaces[j].X, emptySpaces[j].Y, gen.Next(Const.MIN_BACTERIA, Const.MAX_BACTERIA));
					Maps[i].AddTile(new Microsoft.Xna.Framework.Vector2(emptySpaces[j].X, emptySpaces[j].Y), LAYERS.Item, tile);
				}
			}
		}

		/// <summary>
		/// Adds some more bacteria to all maps in the World. Called from a server timer.
		/// </summary>
		/// <param name="param">Since this is a System.Threading.Timer callback, param is an instance of a Server passed as an object</param>
		public static void AddBacteria(object param)
		{
			if (Maps == null)
				return;

			Server serverInstance;
			try
			{
				serverInstance = param as Server;
			}
			catch (InvalidCastException)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error: In World.AddBacteria: Unable to cast server instance from timer callback function!");
				Console.ResetColor();
				return;
			}

			Random gen = new Random();

			for (int i = 0; i < Maps.Count; i++)
			{
				List<System.Drawing.Point> emptySpaces = new List<System.Drawing.Point>(EmptyLocations(i));

				if (Maps[i].Layers[(int)LAYERS.Item].Count < (int)Math.Floor(emptySpaces.Count * Const.FILL_PROPORTION))
				{
					// Add bacteria to map
					int index = gen.Next(0, emptySpaces.Count - 1);
					ItemTile tile = new ItemTile(ItemTileSpec.BACTERIA, emptySpaces[index].X, emptySpaces[index].Y, gen.Next(Const.MIN_BACTERIA, Const.MAX_BACTERIA));
					Maps[i].AddTile(new Microsoft.Xna.Framework.Vector2(emptySpaces[index].X, emptySpaces[index].Y), LAYERS.Item, tile);
					
					ItemData data = new ItemData(tile.Type, tile.X, tile.Y, tile.Quantity);
					if(!serverInstance.SendToClient(ServerAction.Content, data))
					{
						serverInstance.Log(System.Reflection.MethodBase.GetCurrentMethod().Name, "There was an error sending newly added bacteria to clients");
					}
				}
			}
		}
	}
}
