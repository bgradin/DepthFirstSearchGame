using GameClassLibrary;
using System;

namespace GenericMapEditor
{
	internal static class Extensions
	{
		public static int CorrectedTileIndex<T>(this Tileset tileset, int index)
		{
			for (int i = 0, counter = 0; i < tileset.Tiles.Count; i++)
			{
				if (tileset.Tiles[i].GetType() == typeof(T))
					counter++;

				if (counter - 1 == index)
					return i;
			}

			throw new InvalidOperationException("That index doesn't exist!");
		}
	}
}
