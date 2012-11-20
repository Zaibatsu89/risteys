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

namespace XNASimulator
{
    enum TileRotation
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }


    class Tile
    {
        private Texture2D Texture;
        private TileRotation Rotation;

        public Texture2D getTexture()
        {
            return Texture;
        }

        //Dimensions
        private int Height;
        private int Width;
        private Vector2 Size;
        private Vector2 Origin;

        public int getHeight()
        {
            return Height;
        }
        public int getWidth()
        {
            return Width;
        }
        public Vector2 getSize()
        {
            return Size;
        }
        public Vector2 getOrigin()
        {
            return Origin;
        }

        //Information
        public int TileID;
        public int TrafficLightID;
        public Tile[] adjacentTiles;

        public bool isOccupied = false;
        public bool isGreen = false;
        public bool isWalkway = false;

        public Tile(Texture2D texture, TileRotation rotation)
        {
            Height = 64;
            Width = 64;

            Size = new Vector2(Width, Height);
            Origin = new Vector2(Width / 2, Height / 2);

            Texture = texture;
            Rotation = rotation;
        }

        //Calculates rotation of this Tile
        public float getRotation()
        {
            double angle = 0;

            switch(Rotation)
            {
                case TileRotation.Up:
                    angle = 0;
                    break;
                case TileRotation.Right:
                    angle = 90;
                    break;
                case TileRotation.Down:
                    angle = 180;
                    break;
                case TileRotation.Left:
                    angle = 270;
                    break;
            }

            angle = Math.PI * angle / 180.0;
            return (float)angle;
        }

    }
}
