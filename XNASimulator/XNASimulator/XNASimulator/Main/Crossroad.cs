using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNASimulator
{
    class Crossroad
    {
        private Tile[,] tiles;

        public Tile[,] getTiles()
        {
            return tiles;
        }

        //Amount of horizontal tiles
        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        //Amount of vertical tiles
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        public Crossroad(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");         
        }

        public void LoadLevel(string path)
        {
            // Load the level and ensure all of the lines are the same length.
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            // Allocate the tile grid.
            tiles = new Tile[width, lines.Count];

            // Loop over every tile position,
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType, x, y);
                }
            }
        }

        private Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case 'A':
                    return LoadTile("Road64x64", TileRotation.Up);
                case 'a':
                    return LoadTile("Road64x64", TileRotation.Right);
                case 'B':
                    return LoadTile("Crossing64x64", TileRotation.Up);
                case 'b':
                    return LoadTile("Crossing64x64", TileRotation.Right);
                case 'C':
                    return LoadTile("Sidewalk64x64", TileRotation.Up);
                case 'D':
                    return LoadTile("LightsRed64x64", TileRotation.Up);
                case 'd':
                    return LoadTile("LightsRed64x64", TileRotation.Right);
                case 'f':
                    return LoadTile("LightsRed64x64", TileRotation.Left);
                case 'g':
                    return LoadTile("LightsRed64x64", TileRotation.Down);
                case 'S':
                    return LoadTile("Grass64x64", TileRotation.Up);

                // Unknown tile type character
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }
        private Tile LoadTile(string name, TileRotation rotation)
        {
            return new Tile(Content.Load<Texture2D>("Tiles/" + name), rotation);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTiles(spriteBatch);
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
   
            // For each tile position
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // If there is a visible tile in that position
                    Texture2D texture = tiles[x, y].getTexture();
                    if (texture != null)
                    {
                        // Draw it in screen space.
                        Vector2 position = new Vector2(x, y) * tiles[x, y].getSize() + (tiles[x, y].getSize() / 2);
                        spriteBatch.Draw(texture,
                                        position,
                                        null,
                                        Color.White,
                                        tiles[x,y].getRotation(), 
                                        tiles[x,y].getOrigin(), 
                                        1.0f, 
                                        SpriteEffects.None, 
                                        0f);
                    }
                }
            }
        }

        private void DrawVehicles()
        {

        }
    }
}
