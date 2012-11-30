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
using XNASimulator.Globals;

namespace XNASimulator
{
    class Vehicle
    {
        public string ID {get; set;}
        public Texture2D sprite { get; set; }
        public Tile spawntile { get; set; }
        public Vector2 position { get; set; }
        public RotationEnum rotation { get; set; }
        public Vector2 origin { get; set; }
        public Rectangle collission { get; set; }
        public bool alive { get; set; }
        public bool stopCar { get; set; }
        public bool stopRedLight { get; set; }
        
        public Vehicle(Texture2D texture, string ID)
        {
            this.ID = ID;
            rotation = RotationEnum.Up;
            position = Vector2.Zero;
            sprite = texture;
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            alive = false;
            stopCar = false;
            stopRedLight = false;
            collission = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }
    }
}
