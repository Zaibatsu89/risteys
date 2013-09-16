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
        private int DEBUGvehicleID = 0;

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
                    if (tile.isSpawn)
                    {
                        Lane lane;
                        Vehicle newVehicle;

                        lists.Lanes.TryGetValue(tile.laneIDs[0], out lane);

                        newVehicle = lane.AddVehicle(DEBUGvehicleID.ToString());

                        newVehicle.direction = DirectionEnum.NorthToWest;
                        newVehicle.destinationLaneID = "W6";

                        lists.Vehicles.Add(newVehicle);

                        DEBUGvehicleID++;
                    }
                    else
                    {
                        this.ChangeLights(tile);
                    }
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
    }
}