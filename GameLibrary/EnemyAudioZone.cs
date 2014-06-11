using GameClassLibrary;
using System;

namespace GameClassLibrary
{
	public sealed class EnemyAudioZone : AudioZone
	{
		public int Radius { get; private set; }

		public EnemyAudioZone(Enemy enemy, int songIndex, int radius = 100) : base(songIndex)
		{
			m_enemy = enemy;
			Radius = radius;
		}

		public override bool ContainsPlayer(Player mainPlayer)
		{
			int distance = (int) DistanceFromPlayer(mainPlayer);

			return distance * distance < Radius * Radius;
		}

		public float DistanceFromPlayer(Player mainPlayer)
		{
			int deltaX = mainPlayer.X - m_enemy.X;
			int deltaY = mainPlayer.Y - m_enemy.Y;
			return (float) Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
		}

		Enemy m_enemy;
	}
}
