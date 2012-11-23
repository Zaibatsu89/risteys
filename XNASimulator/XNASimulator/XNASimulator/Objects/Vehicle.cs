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
    class Vehicle
    {
        public Texture2D sprite;
        public Tile spawntile;
        public Vector2 position;
        public RotationEnum rotation;
        public Vector2 origin;
        public Vector2 velocity;
        public Rectangle collission;
        public bool alive;
        public bool stopped;
        public bool bumpered;
        
        public Vehicle(Texture2D texture, RotationEnum rotation)
        {
            this.rotation = rotation;
            position = Vector2.Zero;
            sprite = texture;
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            alive = false;
            stopped = false;
            bumpered = false;
            collission = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }
    }
}
