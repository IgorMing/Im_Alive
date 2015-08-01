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
    class Background
    {
        public static bool CanMoveLeft= true;
        public Texture2D texture;
        //public Texture2D door;
        public Rectangle rectangle;
        public Rectangle vector { get; set; }
        public Rectangle recTexture;
        //public Rectangle recDoor;
        public bool movingOnScreen;
        public int positionX;
        public Game Game { get; set; }
        public int Speed { get; set; }
        public bool CanMove { get; set; }

        public Background(string textureName, Rectangle newRectangle, Rectangle newDoorRec)
        {
            positionX = 0;
            this.rectangle = newRectangle;
            //this.recDoor = newDoorRec;
            //this.recDoor.X = 100;
            //this.recDoor.Y = 300;
            texture = Content.Load<Texture2D>(@"Images/Background/" + textureName);
            //door = Content.Load<Texture2D>(@"Images/Background/door");
        }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public void UnloadContent()
        { }

        public void Update(GameTime gameTime)
        {
            recTexture = rectangle;
            if (CanMove)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    positionX += this.Speed;
                    if (movingOnScreen)
                    {
                        recTexture.X -= this.Speed;   
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (rectangle.X < 0)
                    {
                        positionX -= this.Speed;
                        if (movingOnScreen)
                        {
                            recTexture.X += this.Speed;
                            CanMoveLeft = true;
                        }
                    }
                    else
                        Background.CanMoveLeft = false;

                }
            }

            rectangle = recTexture;
        }

        public void Draw(SpriteBatch spbatch, GameTime gameTime)
        {
            //spbatch.Draw(door, recDoor, Color.White);
            while (recTexture.X <= Game.Window.ClientBounds.Width)
            {
                spbatch.Draw(texture, recTexture, Color.White);
                recTexture.X += 1800;
            }

        }

    }
    }
