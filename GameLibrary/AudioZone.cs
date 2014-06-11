using GameClassLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameClassLibrary
{
	public class AudioZone
	{
		public AudioZone(int songIndex) { m_area = null; SongIndex = songIndex; }

		public bool Loaded { get; private set; }
		public int SongIndex { get; set; }

		public void AddArea(Rectangle rect)
		{
			m_area.Add(rect);
		}

		public virtual bool ContainsPlayer(Player mainPlayer)
		{
			if (mainPlayer == null)
				throw new ArgumentNullException("mainPlayer");

			foreach (Rectangle rect in m_area)
			{
				if (rect.ContainsPoint(mainPlayer.X, mainPlayer.Y))
					return true;
			}

			return false;
		}

		List<Rectangle> m_area;
	}
}
