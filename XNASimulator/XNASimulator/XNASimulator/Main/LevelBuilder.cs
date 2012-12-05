using System;
using System.Collections.Generic;
using System.IO;
using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KruispuntGroep6.Simulator.Main
{
    class LevelBuilder
    {
        private Lists lists;

        private int LevelWidth
        {
            get { return lists.Tiles.GetLength(0); }
        } //horizontal tiles
        private int LevelHeight
        {
            get { return lists.Tiles.GetLength(1); }
        } //vertical tiles


        public LevelBuilder(Lists lists)
        {
            this.lists = lists;
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
                while (!string.Equals(line, null))
                {
                    lines.Add(line);
                    if (!Int32.Equals(line.Length, width))
                        throw new Exception(string.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            // Allocate the tile grid.
            lists.Tiles = new Tile[width, lines.Count];

            // Loop over every tile position,
            for (int y = 0; y < LevelHeight; ++y)
            {
                for (int x = 0; x < LevelWidth; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];

                    lists.Tiles[x, y] = LoadTile(tileType, x, y);

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

		private Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case 'A':
                    return LoadTile(Textures.Road, RotationEnum.Up);
                case 'a':
                    return LoadTile(Textures.Road, RotationEnum.Right);
                case 'B':
                    return LoadTile(Textures.Crossing, RotationEnum.Up);
                case 'b':
                    return LoadTile(Textures.Crossing, RotationEnum.Right);
                case 'C':
                    return LoadTile(Textures.SideWalk, RotationEnum.Up);
                case 'D':
                    return LoadTile(Textures.RedLight, RotationEnum.Up);
                case 'd':
                    return LoadTile(Textures.RedLight, RotationEnum.Right);
                case 'f':
                    return LoadTile(Textures.RedLight, RotationEnum.Left);
                case 'g':
                    return LoadTile(Textures.RedLight, RotationEnum.Down);
                case 'S':
                    return LoadTile(Textures.Grass, RotationEnum.Up);
                case 'V':
                    return LoadTile(Textures.Spawn, RotationEnum.Down);
                case 'v':
                    return LoadTile(Textures.Spawn, RotationEnum.Right);

                // Unknown tile type character
                default:
                    throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

		private Tile LoadTile(Texture2D texture, RotationEnum rotation)
        {
			return new Tile(texture, rotation);
        }
    }
}

