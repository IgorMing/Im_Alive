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
    class Monster : Microsoft.Xna.Framework.Game
    {
        SpriteBatch spriteBatch;
        public Game Game { get; set; }
        public AnimationCollection animations;
        public int limitLeft, limitRight;
        public int GameSpeed { get; set; }
        Texture2D running,
          dying,
          idle;
        public int velocity = -2;
        public int positionX, positionY;
        private int initialPosition { set; get; }
        public bool rightToLeft;
        public bool CanMove { get; set; }

        public Monster(ContentManager Content, int initialPosition)
        {
            this.CanMove = true;
            this.initialPosition = initialPosition;
            running = Content.Load<Texture2D>(@"Images/Monsters/MonsterA/Run");
            idle = Content.Load<Texture2D>(@"Images/Monsters/MonsterA/Idle");
            dying = Content.Load<Texture2D>(@"Images/Monsters/MonsterA/Die");
        }

        public void LoadContent(GraphicsDevice graphicsDevice)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            CreateAnimations();
        }

        private void CreateAnimations()
        {
            // Instância, mas também manipulação e criação das animações que serão passadas ao personagem.

            animations = new AnimationCollection();

            Animation run = new Animation(running, 10, 10, 1, 48, 48, new Point(0, 0));
            run.isLoop = true;
            animations.AddAnimation("Correr", run);

            Animation stopped = new Animation(idle, 10, 1, 1, 64, 64, new Point(0, 0));
            stopped.isLoop = false;
            animations.AddAnimation("Parado", stopped);

            Animation die = new Animation(dying, 10, 12, 1, 59, 64, new Point(-3, 0));
            die.isLoop = false;
            animations.AddAnimation("Morrer", die);

            animations.currentAnimation = "Correr";
            animations.position.X = initialPosition;
        }

        public void Update(GameTime gameTime, int limitRight, int limitLeft, int positionY) {

            animations.position.Y = positionY;
            this.positionY = positionY;

            this.limitLeft = limitLeft;
            this.limitRight = limitRight;

            if (CanMove)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    if (rightToLeft)
                        animations.position.X -= GameSpeed;
                    else
                        animations.position.X -= GameSpeed - 2;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (rightToLeft)
                        animations.position.X += GameSpeed - 2;
                    else
                        animations.position.X += GameSpeed;
                }
            }

            this.positionX = (int)animations.position.X;

            movingItself();

            animations.Update(gameTime);

            base.Update(gameTime);
        }

        private void movingItself() {
            if (animations.position.X >= this.limitRight)
                rightToLeft = true;
            else if (animations.position.X <= this.limitLeft)
                rightToLeft = false;

            if (!rightToLeft)
            {
                animations.anotherSide = true;
                animations.position.X += 2;
            }
            else
            {
                animations.anotherSide = false;
                animations.position.X -= 2;
            }
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            animations.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
