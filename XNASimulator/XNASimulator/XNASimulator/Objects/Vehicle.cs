using System;
using KruispuntGroep6.Simulator.Globals;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KruispuntGroep6.Simulator.Objects
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
		public float speed { get; set; }
        public bool stopCar { get; set; }
        public bool stopRedLight { get; set; }
		
        public Vehicle(Texture2D texture, string ID, Random random)
        {
            this.ID = ID;
            rotation = RotationEnum.Up;
            position = Vector2.Zero;
            sprite = texture;
            origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            alive = false;
			while (speed < 0.5f)
			{
				speed = (float)random.NextDouble() * 5f;
			}
            stopCar = false;
            stopRedLight = false;
            collission = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }
    }
}