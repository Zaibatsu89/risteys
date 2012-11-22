using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class Tile
    {
        public Texture2D Texture {get; set;}

        //Positions and Dimensions
        public RotationEnum Rotation { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public Vector2 Size { get; private set; }
        public Vector2 Origin { get; private set; }
        public Vector2 Position {get; set; }
        public Vector2 DrawPosition { get; set; }
        public Rectangle CollisionRectangle { get; private set; }

        //Information
        public int TileID;
        public int TrafficLightID;
        public Tile[] adjacentTiles;

        //Bools
        public bool isOccupied = false;
        public bool isGreen = false;
        public bool isWalkway = false;
        public bool isSpawn = false;

        public Tile(Texture2D texture, RotationEnum rotation)
        {
            this.Height = texture.Height;
            this.Width = texture.Width;

            this.Size = new Vector2(Width, Height);
            this.Origin = new Vector2(Width / 2, Height / 2);

            this.Texture = texture;
            this.Rotation = rotation;
        }

        public void setCollisionRectangle(Vector2 position)
        {
            this.CollisionRectangle = new Rectangle((int)position.X, (int)position.Y, this.Width, this.Height);
        }
    }
}
