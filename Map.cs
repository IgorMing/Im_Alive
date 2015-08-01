using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ImAlive
{
    class Map
    {
        public bool CanMove { get; set; }
        public int Speed { get; set; }
        public bool movingOnScreen;
        private List<CollisionTile> collisionTiles = new List<CollisionTile>();

        public List<CollisionTile> CollisionTiles
        {
            get { return collisionTiles; }
        }

        private int width, height;
        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return Height; }
        }

        public Map() {
            movingOnScreen = false;
        }

        public void Generate(int[,] map, int size)
        {

            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    if (number > 0)
                        collisionTiles.Add(new CollisionTile(number, new Rectangle(x * size, y * size, size, size)));


                    width = (x + 1) * size;
                    height = (y + 1) * size;

                }
        }

        public void Update(GameTime gameTime) 
        {
            if (movingOnScreen)
                for (int i = 0; i < collisionTiles.Count; i++)
                {
                    collisionTiles[i].CanMove = this.CanMove;
                    collisionTiles[i].Speed = this.Speed;
                    collisionTiles[i].Update(gameTime);
                }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            foreach (CollisionTile tile in collisionTiles)
                tile.Draw(spritebatch);
        }


    }
}
