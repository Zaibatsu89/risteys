﻿using System;
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

                while (line != null)
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
                    return LoadTile(Textures.RoadCenter, RotationEnum.North, new Vector2(x,y));
                
                case 'G':
                    return LoadTile(Textures.Grass, RotationEnum.North, new Vector2(x, y));

				#region Crossings
				case 'C':
                    return LoadTile(Textures.Crossing, RotationEnum.North, new Vector2(x, y));
                case 'c':
                    return LoadTile(Textures.Crossing, RotationEnum.West, new Vector2(x, y));
				case 'E':
					return LoadTile(Textures.Crossing, RotationEnum.South, new Vector2(x, y));
				case 'e':
					return LoadTile(Textures.Crossing, RotationEnum.East, new Vector2(x, y));
				#endregion

				#region Sidewalks
				case 's':
					return LoadTile(Textures.Sidewalk, RotationEnum.South, new Vector2(x, y));
				case 'S':
					return LoadTile(Textures.Sidewalk, RotationEnum.East, new Vector2(x, y));
				case 'q':
					return LoadTile(Textures.Sidewalk, RotationEnum.West, new Vector2(x, y));
				case 'Q':
					return LoadTile(Textures.Sidewalk, RotationEnum.North, new Vector2(x, y));
				#endregion

				#region Roads
				case 'A':
                    return LoadTile(Textures.Road, RotationEnum.South, new Vector2(x, y));
                case 'a':
                    return LoadTile(Textures.Road, RotationEnum.East, new Vector2(x, y));
                case 'Z':
                    return LoadTile(Textures.Road, RotationEnum.West, new Vector2(x, y));
                case 'z':
                    return LoadTile(Textures.Road, RotationEnum.North, new Vector2(x, y));
                #endregion

                #region CarSorts
                case 'I':
                    return LoadTile(Textures.CarSortRight, RotationEnum.South, new Vector2(x, y));
                case 'O':
                    return LoadTile(Textures.CarSortDown, RotationEnum.South, new Vector2(x, y));
                case 'P':
                    return LoadTile(Textures.CarSortLeft, RotationEnum.South, new Vector2(x, y));

                case 'i':
                    return LoadTile(Textures.CarSortRight, RotationEnum.East, new Vector2(x, y));
                case 'o':
                    return LoadTile(Textures.CarSortDown, RotationEnum.East, new Vector2(x, y));
                case 'p':
                    return LoadTile(Textures.CarSortLeft, RotationEnum.East, new Vector2(x, y));

                case 'T':
                    return LoadTile(Textures.CarSortRight, RotationEnum.West, new Vector2(x, y));
                case 'Y':
                    return LoadTile(Textures.CarSortDown, RotationEnum.West, new Vector2(x, y));
                case 'U':
                    return LoadTile(Textures.CarSortLeft, RotationEnum.West, new Vector2(x, y));

                case 't':
                    return LoadTile(Textures.CarSortRight, RotationEnum.North, new Vector2(x, y));
                case 'y':
                    return LoadTile(Textures.CarSortDown, RotationEnum.North, new Vector2(x, y));
                case 'u':
                    return LoadTile(Textures.CarSortLeft, RotationEnum.North, new Vector2(x, y));
                #endregion

                #region MiscSorts

                case 'F':
                    return LoadTile(Textures.Bikelane, RotationEnum.South, new Vector2(x, y));
                case 'B':
                    return LoadTile(Textures.Buslane, RotationEnum.South, new Vector2(x, y));

                case 'f':
                    return LoadTile(Textures.Bikelane, RotationEnum.East, new Vector2(x, y));
                case 'b':
                    return LoadTile(Textures.Buslane, RotationEnum.East, new Vector2(x, y));

                case 'D':
                    return LoadTile(Textures.Bikelane, RotationEnum.West, new Vector2(x, y));
                case 'N':
                    return LoadTile(Textures.Buslane, RotationEnum.West, new Vector2(x, y));

                case 'd':
                    return LoadTile(Textures.Bikelane, RotationEnum.North, new Vector2(x, y));
                case 'n':
                    return LoadTile(Textures.Buslane, RotationEnum.North, new Vector2(x, y));

                #endregion

                #region SidewalkLights
                case 'V':
                    return LoadTile(Textures.Sidewalk2Red, RotationEnum.North, new Vector2(x, y));
                case '>':
                    return LoadTile(Textures.Sidewalk2Red, RotationEnum.East, new Vector2(x, y));
                case 'v':
                    return LoadTile(Textures.Sidewalk2Red, RotationEnum.South, new Vector2(x, y));
                case '<':
                    return LoadTile(Textures.Sidewalk2Red, RotationEnum.West, new Vector2(x, y));

                #endregion

                #region TrafficLights

                case 'L':
                    return LoadTile(Textures.RedLight, RotationEnum.South, new Vector2(x, y));
                case 'l':
                    return LoadTile(Textures.RedLight, RotationEnum.East, new Vector2(x, y));
                case 'K':
                    return LoadTile(Textures.RedLight, RotationEnum.West, new Vector2(x, y));
                case 'k':
                    return LoadTile(Textures.RedLight, RotationEnum.North, new Vector2(x, y));

                #endregion
                



                // Unknown tile type character
                default:
                    throw new NotSupportedException(string.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

		private Tile LoadTile(Texture2D texture, RotationEnum rotation, Vector2 gridposition)
        {
			return new Tile(texture, rotation, gridposition);
        }
    }
}

