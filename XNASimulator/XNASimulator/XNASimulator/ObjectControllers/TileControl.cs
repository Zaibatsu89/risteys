using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KruispuntGroep6.Simulator.Main;
using XNASimulator.Globals;

namespace KruispuntGroep6.Simulator.ObjectControllers
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

        public void ChangeLights(Tile tile)
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
        public void ChangeLights(Tile tile, LightsEnum colour)
        {
            switch (colour)
            {
                case LightsEnum.Blink: tile.Texture = Textures.BlinkLight;
                    break;
                case LightsEnum.Red: tile.Texture = Textures.RedLight;
                    break;
                case LightsEnum.Green: tile.Texture = Textures.GreenLight;
                    break;
                case LightsEnum.Yellow: tile.Texture = Textures.YellowLight;
                    break;
            }
        }

        public void FillTileList()
        {
            foreach (Tile tile in lists.Tiles)
            {
                int tileX = (int)tile.GridCoordinates.X;
                int tileY = (int)tile.GridCoordinates.Y;

                if (tileX > 0)
                {
                    tile.adjacentTiles.Add(RotationEnum.West.ToString(), lists.Tiles[tileX - 1, tileY]);
                }
                if (tileX < MainGame.TilesHor - 1)
                {
                    tile.adjacentTiles.Add(RotationEnum.East.ToString(), lists.Tiles[tileX + 1, tileY]);
                }
                if (tileY > 0)
                {
                    tile.adjacentTiles.Add(RotationEnum.North.ToString(), lists.Tiles[tileX, tileY - 1]);
                }
                if (tileY < MainGame.TilesVer - 1)
                {
                    tile.adjacentTiles.Add(RotationEnum.South.ToString(), lists.Tiles[tileX, tileY + 1]);
                }
                
            }
        }
    }
}