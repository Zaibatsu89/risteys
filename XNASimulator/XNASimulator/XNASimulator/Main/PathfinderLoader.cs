using System;
using System.Collections.Generic;
using System.IO;
using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.Objects;
using Microsoft.Xna.Framework;

namespace KruispuntGroep6.Simulator.Main
{
	class PathfinderLoader
	{
		private int[,] layout;

		private Lists lists;

		private int Width
		{
			get { return lists.Tiles.GetLength(0); }
		} //horizontal tiles
		private int Height
		{
			get { return lists.Tiles.GetLength(1); }
		} //vertical tiles

		public PathfinderLoader(Lists lists)
		{
			this.lists = lists;
		}

		/// <summary>
		/// Returns the tile index for the given cell.
		/// </summary>
		public int GetIndex(int cellX, int cellY)
		{
			if (cellX < 0 || cellX > Width - 1 || cellY < 0 || cellY > Height - 1)
				return 0;

			return layout[cellX, cellY];
		}

		public void LoadLevel(string path)
		{
			// Load the level and ensure all of the lines are the same length.
			int width;
			List<string> lines = new List<string>();
			Tile tile;
			Vector2 drawposition;
			Vector2 position;

			using (StreamReader reader = new StreamReader(path))
			{
				string line = reader.ReadLine();
				width = line.Length;

				while (line != null)
				{
					lines.Add(line);
					if (!int.Equals(line.Length, width))
						throw new Exception(string.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
					line = reader.ReadLine();
				}
			}

			this.layout = new int[Width, Height];

			// Loop over every tile position,
			for (int y = 0; y < Height; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					// to load each tile
					char tileType = lines[x][y];

					LoadTile(tileType, x, y);

					// set positions
					tile = lists.Tiles[x, y];

					position = new Vector2(x, y) * tile.Size;
					drawposition = position + (tile.Size / 2);

					tile.Position = position;
					tile.DrawPosition = drawposition;
					tile.CollisionRectangle = new Rectangle((int)position.X, (int)position.Y, tile.Width, tile.Height);
				}
			}
		}

		private void LoadTile(char tileType, int x, int y)
		{
			switch (tileType)
			{
				case '0':
					layout[x, y] = 0;
					break;
				case '1':
					layout[x, y] = 1;
					break;
				// Unknown tile type character
				default:
					throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
			}
		}

		public int[,] GetLayout()
		{
			return layout;
		}
	}
}