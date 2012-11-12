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
    class Vehicle
    {
        public Texture2D sprite;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;
        
        public Vehicle(Texture2D texture, float rotation)
        {
            this.rotation = rotation;
            position = Vector2.Zero;
            sprite = texture;
            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            velocity = Vector2.Zero;
            alive = false;
        }
    }
}
