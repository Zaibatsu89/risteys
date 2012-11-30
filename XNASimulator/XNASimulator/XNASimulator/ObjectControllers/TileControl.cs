using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNASimulator.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNASimulator.Globals;

namespace XNASimulator.ObjectControllers
{
    class TileControl
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

        public TileControl(Lists lists)
        {
            this.lists = lists;
        }

        public void Update(GameTime gameTime)
        {
            this.UpdateTileOccupied();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // For each tile position
            for (int y = 0; y < LevelHeight; ++y)
            {
                for (int x = 0; x < LevelWidth; ++x)
                {
                    // If there is a visible tile in that position
                    if (!Texture2D.Equals(lists.Tiles[x, y].Texture, null))
                    {
                        // Draw it in screen space.                     
                        spriteBatch.Draw(lists.Tiles[x, y].Texture,
                                        lists.Tiles[x, y].DrawPosition,
                                        null,
                                        Color.White,
                                        Rotation.getRotation(lists.Tiles[x, y].Rotation),
                                        lists.Tiles[x, y].Origin,
                                        1.0f,
                                        SpriteEffects.None,
                                        1.0f);
                    }
                }
            }
        }

        public void CheckMouseCollision(Vector2 mouseposition)
        {
            Rectangle mouseArea = new Rectangle((int)mouseposition.X, (int)mouseposition.Y, 1, 1);

            foreach (Tile tile in lists.Tiles)
            {
                if (tile.CollisionRectangle.Contains(mouseArea))
                {
                    this.ChangeLights(tile);
                }
            }
        }

        private void UpdateTileOccupied()
        {
            foreach (Tile tile in lists.Tiles)
            {
                foreach (Vehicle vehicle in lists.Vehicles)
                {
                    //check if vehicle is the one occupying the tile...
                    if (vehicle.ID == tile.OccupiedID)
                    {
                        //if occupying vehicle no longer occupies...
                        if (!vehicle.collission.Intersects(tile.CollisionRectangle))
                        {
                            //release tile
                            tile.isOccupied = false;
                            tile.OccupiedID = "";
                        }
                    }

                    //if not occupied and vehicle enters...
                    if (!tile.isOccupied && vehicle.collission.Intersects(tile.CollisionRectangle))
                    {
                        tile.isOccupied = true;
                        tile.OccupiedID = vehicle.ID;
                    }
                }
            }
        }

        private void ChangeLights(Tile tile)
        {
            if (tile.isGreen == false && tile.Texture.Equals(Textures.RedLight))
            {
                tile.Texture = Textures.GreenLight;
                tile.isGreen = true;
            }
            else if (tile.isGreen == true && tile.Texture.Equals(Textures.GreenLight))
            {
                tile.Texture = Textures.RedLight;
                tile.isGreen = false;
            }
        }
    }
}
