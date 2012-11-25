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
using XNASimulator.Enums;

namespace XNASimulator
{
    class Crossroad
    {
        public Tile[,] tiles { get; private set; }
        public ContentManager Content { get; private set; }

        public int Width
        {
            get { return tiles.GetLength(0); }
        } //horizontal tiles
        public int Height
        {
            get { return tiles.GetLength(1); }
        } //vertical tiles

        public Crossroad(IServiceProvider serviceProvider)
        {
            Content = new ContentManager(serviceProvider, "Content");         
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

                    // set positions 
                    tile = tiles[x, y];

                    position = new Vector2(x, y) * tile.Size;
                    drawposition = position + (tile.Size / 2);

                    tile.Position = position;
                    tile.DrawPosition = drawposition;
                    tile.setCollisionRectangle(position);
                }
            }
        }


        public void CheckMouseCollision(Vector2 mouseposition)
        {
            Rectangle mouseArea = new Rectangle((int)mouseposition.X, (int)mouseposition.Y, 1, 1);

            foreach (Tile tile in tiles)
            {
                if (tile.CollisionRectangle.Contains(mouseArea))
                {
                    this.ChangeLights(tile);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTiles(spriteBatch);
        }

        private Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case 'A':
                    return LoadTile("Road64x64", RotationEnum.Up);
                case 'a':
                    return LoadTile("Road64x64", RotationEnum.Right);
                case 'B':
                    return LoadTile("Crossing64x64", RotationEnum.Up);
                case 'b':
                    return LoadTile("Crossing64x64", RotationEnum.Right);
                case 'C':
                    return LoadTile("Sidewalk64x64", RotationEnum.Up);
                case 'D':
                    return LoadTile("LightsRed64x64", RotationEnum.Up);
                case 'd':
                    return LoadTile("LightsRed64x64", RotationEnum.Right);
                case 'f':
                    return LoadTile("LightsRed64x64", RotationEnum.Left);
                case 'g':
                    return LoadTile("LightsRed64x64", RotationEnum.Down);
                case 'S':
                    return LoadTile("Grass64x64", RotationEnum.Up);
                case 'V':
                    return LoadTile("Spawn64x64", RotationEnum.Down);
                case 'v':
                    return LoadTile("Spawn64x64", RotationEnum.Right);

                // Unknown tile type character
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

        private Tile LoadTile(string name, RotationEnum rotation)
        {
            return new Tile(Content.Load<Texture2D>("Tiles/" + name), rotation);
        }

        private void DrawTiles(SpriteBatch spriteBatch)
        {
   
            // For each tile position
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // If there is a visible tile in that position
                    if (!Texture2D.Equals(tiles[x, y].Texture, null))
                    {
                        // Draw it in screen space.                     
                        spriteBatch.Draw(tiles[x,y].Texture,
                                        tiles[x,y].DrawPosition, 
                                        null,
                                        Color.White,
                                        Rotation.getRotation(tiles[x,y].Rotation), 
                                        tiles[x,y].Origin, 
                                        1.0f, 
                                        SpriteEffects.None, 
                                        1.0f);
                    }
                }
            }
        }

        private void ChangeLights(Tile tile)
        {
            Texture2D redLightTexture = Content.Load<Texture2D>("Tiles/LightsRed64x64");
            Texture2D greenLightTexture = Content.Load<Texture2D>("Tiles/LightsGreen64x64");

            if (tile.isGreen == false && tile.Texture.Equals(redLightTexture))
            {
                tile.Texture = greenLightTexture;
                tile.isGreen = true;
            }
            else if (tile.isGreen == true && tile.Texture.Equals(greenLightTexture))
            {
                tile.Texture = redLightTexture;
                tile.isGreen = false;
            }
        }
    }
}
