using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
	public class Player
	{
		// Current location
		public int X { get; set; }
		public int Y { get; set; }

		// Offset from current point, in pixels
		public int OffsetX { get; set; }
		public int OffsetY { get; set; }
		public int StartingOffset { get; private set; }

		// Coordinates adjusted for offset
		public int ActualX
		{
			get
			{
				int actualX = X * Const.TILE_SIZE;

				return actualX + OffsetX;
			}
			private set
			{
				if (MovementBlocked)
					OffsetX += (value - X) * Const.TILE_SIZE;
			}
		}
		public int ActualY
		{
			get
			{
				int actualY = Y * Const.TILE_SIZE;

				return actualY + OffsetY;
			}
			private set
			{
				if (MovementBlocked)
					OffsetY += (value - Y) * Const.TILE_SIZE;
			}
		}

		public Queue<Keys> NextMoves { get; set; }

		public int Radius
		{
			get { return Const.MIN_PLAYER_RADIUS + BacteriaCount; }
		}

		public int CurrentMap { get; set; }

		int bacteriaCount = 0;
		public int BacteriaCount
		{
			get { return bacteriaCount; }
			set { bacteriaCount = value; }
		}

		public int AdminLevel { get; protected set; }

		public string UserName { get; set; }

		public bool MovementBlocked { get; set; } // true if the player is unable to move, false otherwise

		public int CurrentGraphicIndex { get; set; }
		public int FrontGraphicIndex { get; set; }
		public int BackGraphicIndex { get; set; }
		public int LeftGraphicIndex { get; set; }
		public int RightGraphicIndex { get; set; }

		public Player()
		{
			X = Y = -1;
			NextMoves = new Queue<Keys>();
		}

		public Player(string username, int currentMap, int x, int y, int adminLevel)
		{
			UserName = username;
			CurrentMap = currentMap;
			X = x;
			Y = y;
			AdminLevel = adminLevel;
			NextMoves = new Queue<Keys>();
		}

		public void StartMoving(Direction direction, int distance, Point? previousLocation = null)
		{
			MovementBlocked = true;
			if (!Enum.TryParse(direction.ToString(), out m_currentKey))
				throw new Exception("Invalid direction value passed in!");

			if (OffsetX == 0 && OffsetY == 0)
				StartingOffset = 0;

			if (previousLocation != null && previousLocation.Equals(new Point(X, Y)))
			{
				X = previousLocation.Value.X;
				Y = previousLocation.Value.Y;
			}

			if (direction == Direction.Left || direction == Direction.Right)
			{
				if (direction == Direction.Left)
				{
					CurrentGraphicIndex = LeftGraphicIndex;
					distance *= -1;
				}
				else
					CurrentGraphicIndex = RightGraphicIndex;

				int currentPosition = X;
				X += distance;
				ActualX = currentPosition;
				StartingOffset += OffsetX;
			}
			else
			{
				if (direction == Direction.Up)
				{
					CurrentGraphicIndex = BackGraphicIndex;
					distance *= -1;
				}
				else
					CurrentGraphicIndex = FrontGraphicIndex;

				int currentPosition = Y;
				Y += distance;
				ActualY = currentPosition;
				StartingOffset += OffsetY;
			}
		}

		public void UpdateMovement()
		{
			if (OffsetX != 0 || OffsetY != 0)
			{
				int correctedPPM = pixelsPerMovement;
				int offset = OffsetY == 0 ? OffsetX : OffsetY;

				for (int i = 4; i <= Math.Pow(2, pixelsPerMovement); i *= 2)
				{
					if (Math.Abs(offset) <= Const.TILE_SIZE / i)
						correctedPPM--;
					if (Math.Abs(offset) >= Math.Abs(StartingOffset) - (Const.TILE_SIZE / i))
						correctedPPM--;
				}

				try
				{
					if (OffsetY == 0)
					{
						int addValue = (Math.Abs(StartingOffset) / StartingOffset) * correctedPPM;
						OffsetX -= addValue;

						if (OffsetX == 0 || (OffsetX > 0 && StartingOffset < 0) || (OffsetX < 0 && StartingOffset > 0))
						{
							StartingOffset = OffsetX = 0;
							MovementBlocked = false;
						}
					}
					else
					{
						int addValue = (Math.Abs(StartingOffset) / StartingOffset) * correctedPPM;
						OffsetY -= addValue;

						if (OffsetY == 0 || (OffsetY > 0 && StartingOffset < 0) || (OffsetY < 0 && StartingOffset > 0))
						{
							StartingOffset = OffsetY = 0;
							MovementBlocked = false;
						}
					}
				}
				catch (DivideByZeroException)
				{
					return;
				}

				if (Math.Abs(offset) <= Const.TILE_SIZE / 4)
					MovementBlocked = false;
			}
			else
				StartingOffset = 0;
		}

		public int GetMove(Map currentMap, out Keys? key)
		{
			key = null;
			int dist = 0;

			if (NextMoves.Count > 0)
			{
				dist = c_playerStep;

				switch (key = NextMoves.Dequeue())
				{
					case Keys.Up:
						if (Y == 0)
							dist = 0;
						break;

					case Keys.Down:
						if (Y == currentMap.Height - 1)
							dist = 0;
						break;

					case Keys.Right:
						if (X == currentMap.Width - 1)
							dist = 0;
						break;

					case Keys.Left:
						if (X == 0)
							dist = 0;
						break;
				}
			}

			return dist;
		}

		Keys m_currentKey;

		const int c_playerStep = 1;
		const int pixelsPerMovement = 4;
	}
}
