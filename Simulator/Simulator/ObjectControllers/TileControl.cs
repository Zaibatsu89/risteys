using KruispuntGroep6.Simulator.Globals;
using KruispuntGroep6.Simulator.Main;
using KruispuntGroep6.Simulator.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KruispuntGroep6.Simulator.ObjectControllers
{
    public class TileControl
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
                        spriteBatch.Draw
						(
							lists.Tiles[x, y].Texture,
                            lists.Tiles[x, y].DrawPosition,
                            null,
                            Color.White,
                            Rotation.getRotation(lists.Tiles[x, y].Rotation),
                            lists.Tiles[x, y].Origin,
                            1.0f,
                            SpriteEffects.None,
                            1.0f
						);
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
			 if (tile.isGreen)
			 {
				if (tile.Texture.Equals(Textures.GreenLight))
				{
					tile.Texture = Textures.RedLight;
				}
				else if (tile.Texture.Equals(Textures.Sidewalk2Green))
				{
					tile.Texture = Textures.Sidewalk2Red;
				}
                
                tile.isGreen = false;
            }
            else
			{
				if (tile.Texture.Equals(Textures.RedLight))
				{
					tile.Texture = Textures.GreenLight;
				}
				else if (tile.Texture.Equals(Textures.Sidewalk2Red))
				{
					tile.Texture = Textures.Sidewalk2Green;
				}
                
                tile.isGreen = true;
            }
        }

        public void ChangeLights(string laneID, LightsEnum colour)
        {
            foreach (Lane lane in lists.Lanes)
            {
                if(laneID.Equals(lane.laneID))
                {
					switch (colour)
					{
						case LightsEnum.Blink:
							if (!laneID[1].Equals('0') && !laneID[1].Equals('7'))
							{
								lane.trafficLight.Texture = Textures.BlinkLight;
							}
							break;
						case LightsEnum.Green:
							if (laneID[1].Equals('0') || laneID[1].Equals('7'))
							{
								if (laneID[1].Equals('0'))
								{
									if (lane.trafficLight.Texture.Equals(Textures.SidewalkDownRed))
									{
										lane.trafficLight.Texture = Textures.Sidewalk2Green;
									}
									else
									{
										lane.trafficLight.Texture = Textures.SidewalkRightRed;
									}
								}
								else
								{
									if (lane.trafficLight.Texture.Equals(Textures.SidewalkRightRed))
									{
										lane.trafficLight.Texture = Textures.Sidewalk2Green;
									}
									else
									{
										lane.trafficLight.Texture = Textures.SidewalkDownRed;
									}
								}
							}
							else
							{
								lane.trafficLight.Texture = Textures.GreenLight;
							}
							break;
						case LightsEnum.Off:
							if (laneID[1].Equals('0') || laneID[1].Equals('7'))
							{
								lane.trafficLight.Texture = Textures.Sidewalk2Red;
							}
							else
							{
								lane.trafficLight.Texture = Textures.RedLight;
							}
							break;
						case LightsEnum.Red:
							if (laneID[1].Equals('0') || laneID[1].Equals('7'))
							{
								if (laneID[1].Equals('0'))
								{
									if (lane.trafficLight.Texture.Equals(Textures.SidewalkRightRed))
									{
										lane.trafficLight.Texture = Textures.Sidewalk2Red;
									}
									else
									{
										lane.trafficLight.Texture = Textures.SidewalkDownRed;
									}
								}
								else
								{
									if (lane.trafficLight.Texture.Equals(Textures.SidewalkDownRed))
									{
										lane.trafficLight.Texture = Textures.Sidewalk2Red;
									}
									else
									{
										lane.trafficLight.Texture = Textures.SidewalkRightRed;
									}
								}
							}
							else
							{
								lane.trafficLight.Texture = Textures.RedLight;
							}
							break;
						case LightsEnum.Yellow:
							if (!laneID[1].Equals('0') && !laneID[1].Equals('7'))
							{
								lane.trafficLight.Texture = Textures.YellowLight;
							}
							break;
					}
				}
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