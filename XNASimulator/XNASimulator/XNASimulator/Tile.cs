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
        Left = 270,
    }

    struct Tile
    {
        public Texture2D Texture;
        public TileRotation Rotation;

        float circle = MathHelper.Pi * 2;
        //private int rotationAngle;

        public const int Height = 128;
        public const int Width = 128;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(Texture2D texture, TileRotation rotation)
        {
            //rotationAngle = rotation. % circle;
            Texture = texture;
            Rotation = rotation;
        }
    }
}
