using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace ImAlive
{
    class Floor
    {
        public Texture2D texture;
//        public Rectangle rectangle;
        public Vector2 Vector { get; set; }
        public Vector2 vector;
        public Game Game { get; set; }
        public int Speed { get; set; }
        public bool CanMove { get; set; }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public Floor()
        {
            this.texture = Content.Load<Texture2D>(@"Images/Ground/ground");
        }

        public void LoadContent()
        {
            Vector = new Vector2(0, Game.Window.ClientBounds.Height - 32);
        }

        public void UnloadContent()
        { }

        public void Update(GameTime gameTime)
        {
            vector = Vector; // atentar nessa linha, 

            if (CanMove)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    vector.X -= this.Speed;

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (vector.X < 0)
                    {
                        vector.X += this.Speed;
                        Background.CanMoveLeft = true;
                    }
                    else
                        Background.CanMoveLeft = false;

                }
            }

            Vector = vector;
        }

        public void Draw(SpriteBatch spbatch, GameTime gameTime)
        {
            while (vector.X <= Game.Window.ClientBounds.Width)
            {
                spbatch.Draw(texture, vector, Color.White);
                vector.X += 350;
            }
        }

    }


}
