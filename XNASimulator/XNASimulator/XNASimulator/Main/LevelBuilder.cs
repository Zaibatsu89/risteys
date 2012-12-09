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
                case 'X':
                    return LoadTile(Textures.RoadCenter, RotationEnum.Up);
                case 'S':
                    return LoadTile(Textures.Sidewalk, RotationEnum.Up);
                case 'G':
                    return LoadTile(Textures.Grass, RotationEnum.Up);
                case 'C':
                    return LoadTile(Textures.Crossing, RotationEnum.Up);
                case 'c':
                    return LoadTile(Textures.Crossing, RotationEnum.Right);

                #region Roads
                case 'A':
                    return LoadTile(Textures.Road, RotationEnum.Down);
                case 'a':
                    return LoadTile(Textures.Road, RotationEnum.Right);
                case 'Z':
                    return LoadTile(Textures.Road, RotationEnum.Left);
                case 'z':
                    return LoadTile(Textures.Road, RotationEnum.Up);
                #endregion

                #region CarSorts
                case 'I':
                    return LoadTile(Textures.CarSortRight, RotationEnum.Down);
                case 'O':
                    return LoadTile(Textures.CarSortDown, RotationEnum.Down);
                case 'P':
                    return LoadTile(Textures.CarSortLeft, RotationEnum.Down);

                case 'i':
                    return LoadTile(Textures.CarSortRight, RotationEnum.Right);
                case 'o':
                    return LoadTile(Textures.CarSortDown, RotationEnum.Right);
                case 'p':
                    return LoadTile(Textures.CarSortLeft, RotationEnum.Right);

                case 'T':
                    return LoadTile(Textures.CarSortRight, RotationEnum.Left);
                case 'Y':
                    return LoadTile(Textures.CarSortDown, RotationEnum.Left);
                case 'U':
                    return LoadTile(Textures.CarSortLeft, RotationEnum.Left);

                case 't':
                    return LoadTile(Textures.CarSortRight, RotationEnum.Up);
                case 'y':
                    return LoadTile(Textures.CarSortDown, RotationEnum.Up);
                case 'u':
                    return LoadTile(Textures.CarSortLeft, RotationEnum.Up);
                #endregion

                #region MiscSorts

                case 'F':
                    return LoadTile(Textures.Bikelane, RotationEnum.Down);
                case 'B':
                    return LoadTile(Textures.Buslane, RotationEnum.Down);

                case 'f':
                    return LoadTile(Textures.Bikelane, RotationEnum.Right);
                case 'b':
                    return LoadTile(Textures.Buslane, RotationEnum.Right);

                case 'D':
                    return LoadTile(Textures.Bikelane, RotationEnum.Left);
                case 'N':
                    return LoadTile(Textures.Buslane, RotationEnum.Left);

                case 'd':
                    return LoadTile(Textures.Bikelane, RotationEnum.Up);
                case 'n':
                    return LoadTile(Textures.Buslane, RotationEnum.Up);

                #endregion

                #region SidewalkLights
                case 'V':
                    return LoadTile(Textures.Sidewalk2Red, RotationEnum.Up);
                case '>':
                    return LoadTile(Textures.Sidewalk2Red, RotationEnum.Right);
                case 'v':
                    return LoadTile(Textures.Sidewalk2Red, RotationEnum.Down);
                case '<':
                    return LoadTile(Textures.Sidewalk2Red, RotationEnum.Left);

                #endregion

                #region TrafficLights

                case 'L':
                    return LoadTile(Textures.RedLight, RotationEnum.Down);
                case 'l':
                    return LoadTile(Textures.RedLight, RotationEnum.Right);
                case 'K':
                    return LoadTile(Textures.RedLight, RotationEnum.Left);
                case 'k':
                    return LoadTile(Textures.RedLight, RotationEnum.Up);

                #endregion
                



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

