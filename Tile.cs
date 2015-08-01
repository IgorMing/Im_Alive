using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ImAlive
{
    class Tile
    {
        protected Texture2D texture;
        public Rectangle rectangle;
        public int Speed { get; set; }
        public bool CanMove { get; set; }

        public Rectangle GetRetangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }

        }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if (CanMove)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    this.rectangle.X += -this.Speed;
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && Background.CanMoveLeft)
                    this.rectangle.X += this.Speed;
            }
        }

    }


    class CollisionTile : Tile
    {
        public CollisionTile(int i, Rectangle newrectangle)
        {
            this.CanMove = true;
            texture = Content.Load<Texture2D>(@"Images/Blocks/BlockA" + i);
            this.GetRetangle = newrectangle;
        }

    }


}
