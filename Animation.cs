using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ImAlive
{
    class Animation
    {
        public Texture2D texture;
        public int framesPerSecond;
        private float timePerFrame;
        private float totalElapsedTime;
        public Point frameSize;
        public Point currentFrame;
        public Point sheetSize;
        public Point initialPosition;
        public bool isLoop;
        public Vector2 position;
        public Rectangle rectangle;
        public Rectangle Rectangle { get {return rectangle; } set { rectangle = value;} }

        public Animation(Texture2D texture, int fps, int y, int x, int width, int height, Point initial)
        {
            this.texture = texture;
            this.framesPerSecond = fps;
            this.timePerFrame = 1 / (float)fps;
            this.sheetSize = new Point(y, x);
            this.frameSize = new Point(width, height);
            this.currentFrame = new Point(0, 0);
            this.isLoop = false;
            this.initialPosition = initial;
        }

        public void Update(GameTime gameTime)
        {
            totalElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (totalElapsedTime > timePerFrame)
            {
                currentFrame.X++;
                if (currentFrame.X >= sheetSize.X)
                {
                    if (isLoop)
                        currentFrame.X = 0;
                    else
                        currentFrame.X--;

                }
            
                  
                currentFrame.Y++;
                if (currentFrame.Y >= sheetSize.Y)
                {
                    if (isLoop)
                        currentFrame.Y = 0;
                    else
                        currentFrame.Y--;
                }
            totalElapsedTime = 0;
            }
            this.rectangle = new Rectangle(
                                    initialPosition.X + (currentFrame.X * frameSize.X),
                                        initialPosition.Y + (currentFrame.Y * frameSize.Y),
                                            frameSize.X, frameSize.Y);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool anotherSide)
        {
            this.position = position;
            spriteBatch.Draw(texture,this.position, this.rectangle, Color.White, 0, 
                            Vector2.Zero, 1, !anotherSide ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }
    }
}
