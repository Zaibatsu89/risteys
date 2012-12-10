using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KruispuntGroep6.Simulator.Main
{
    /// <summary>
    /// A physical representation of the map.
    /// </summary>
    public class Map
    {
		private int[,] layout;
        private List<Texture2D> textures;

        /// <summary>
        /// The width of the map.
        /// </summary>
        public int Width
        {
            get { return layout.GetLength(1); }
        }
        /// <summary>
        /// The height of the map.
        /// </summary>
        public int Height
        {
            get { return layout.GetLength(0); }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Map(int[,] layout)
        {
			this.layout = layout;
        }

        /// <summary>
        /// Sets the textures for the map to draw.
        /// </summary>
        public void SetTextures(List<Texture2D> textures)
        {
            this.textures = textures;
        }

        /// <summary>
        /// Returns the tile index for the given cell.
        /// </summary>
        public int GetIndex(int cellX, int cellY)
        {
            if (cellX < 0 || cellX > Width - 1 || cellY < 0 || cellY > Height - 1)
                return 0;

            return layout[cellY, cellX];
        }

        /// <summary>
        /// Draws the map.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (textures == null)
            {
                return;
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int index = layout[y, x];

                    spriteBatch.Draw(textures[index], new Vector2(x, y) 
                        * 32, Color.White);
                }
            }
        }

		public void SetPath(Vector2 pos)
		{
			layout[(int)pos.Y / 32, (int)pos.X / 32] = 2;
		}
    }
}
