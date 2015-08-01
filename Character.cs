using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ImAlive
{
    class Character : Microsoft.Xna.Framework.Game
    {
        SpriteBatch spriteBatch;
        public AnimationCollection animations;
        public Game Game { get; set;}
        Texture2D running,
          jumping,
          dying,
          idle,
          celebrating;
        public Vector2 velocity;
        public bool isJumping;
        public bool isMoving;
        public bool movingOnScreen;
        private bool beginNow;
        public bool isDead;
        public bool canMoveLeft;
        public Rectangle stopToFall;
        public bool isFalling;

        public bool CanMove { get; set; }
        public int Speed { get; set; }
        float jumpPower { get; set; }
        
        public Character(ContentManager Content)
        {
            this.canMoveLeft = false;
            this.CanMove = true;
            this.jumpPower = 15f;
            isFalling = false;
            movingOnScreen = true;
            running = Content.Load<Texture2D>(@"Images/Character/Run");
            celebrating = Content.Load<Texture2D>(@"Images/Character/Celebrate");
            idle = Content.Load<Texture2D>(@"Images/Character/Idle");
            jumping = Content.Load<Texture2D>(@"Images/Character/Jump");
            dying = Content.Load<Texture2D>(@"Images/Character/Die");

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

            Animation run = new Animation(running, 10, 10, 1, 64, 64, new Point(0, 0));
            run.isLoop = true;
            animations.AddAnimation("Correr", run);

            Animation celebrate = new Animation(celebrating, 10, 11, 1, 64, 64, new Point(0, 0));
            celebrate.isLoop = true;
            animations.AddAnimation("Celebrar", celebrate);

            Animation jump = new Animation(jumping, 10, 10, 1, 64, 64, new Point(0, 0));
            jump.isLoop = true;
            animations.AddAnimation("Pular", jump);

            Animation stopped = new Animation(idle, 10, 1, 1, 64, 64, new Point(0, 0));
            stopped.isLoop = false;
            animations.AddAnimation("Parado", stopped);

            Animation die = new Animation(dying, 10, 12, 1, 59, 64, new Point(-3, 0));
            die.isLoop = false;
            animations.AddAnimation("Morrer", die);

            beginNow = true;
            animations.currentAnimation = "Parado";
        }

        public Animation GetAnimation()
        {
            return animations.GetAnimation();
        }

        protected override void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            // Seta posição inicial
            if (beginNow)
            {
                animations.position = new Vector2(0, 480 - 64);
                beginNow = false;
            }

            // Se estiver morto, seta a possibilidade de movimentação como false.
            if (this.isDead) {
                this.CanMove = false;
                animations.ChangeAnimation("Morrer");
            }

            // Morrer quando cair em algum buraco.
            if (GetAnimation().position.Y > 480 - 64)
            {
                this.isDead = true;
                this.CanMove = false;
                animations.ChangeAnimation("Morrer");
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Permite a movimentação
            if ((!isDead) && CanMove)
            {
                animations.position += velocity;
                animations.ChangeAnimation("Parado");
                isMoving = false;

                // Para direita
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    if (!isJumping)
                        animations.ChangeAnimation("Correr");
                    if (movingOnScreen && animations.position.X <= 720)
                        velocity.X = +Speed;
                    else
                        velocity.X = 0;
                    animations.anotherSide = true;
                    isMoving = true;
                }
                else
                    // Para esquerda
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        if (!isJumping)
                            animations.ChangeAnimation("Correr");
                        animations.anotherSide = false;
                        isMoving = true;
                        if ((walkOnScreen() && animations.position.X > 0) || (canMoveLeft && animations.position.X > 0))
                            velocity.X = -Speed;
                        else
                            velocity.X = 0;
                    }
                    else
                        velocity.X = 0;

                // Pular
                if (!isJumping && !isFalling)
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                        if (!isJumping)
                        {
                            animations.ChangeAnimation("Pular");
                            animations.position.Y -= 10f; // posição Y é decrementada, ou seja, ele sobe.
                            velocity.Y = -jumpPower;  // força para o pulo. Quanto maior, mais alto o pulo!
                            isJumping = true;
                            isFalling = true;
                        }

                // Condição para cair, ou não.
                if (isJumping || stopToFall.Y > animations.position.Y)
                {
                    this.animations.ChangeAnimation("Pular");
                    falling();
                }
                else //if (isFalling || animations.position.Y >= stopToFall.Y)
                {
                    velocity.Y = 0f;
                }
            }
            else if(isDead) // se morrer
            {
                this.CanMove = false;
                animations.ChangeAnimation("Morrer");
            }

            // Até onde irá cair.
            if (!isFalling)
            {
                isJumping = false;
                if (between((int)animations.position.Y, stopToFall.Y, stopToFall.Y + 32))
                {
                    animations.position.Y = stopToFall.Y;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                this.isDead = false;
                this.CanMove = true;
                animations.GetAnimation().currentFrame = new Point(0, 0);
            }

            if (nearTile() && !isJumping && velocity.Y > 0)
                this.animations.position.Y = stopToFall.Y;

            animations.Update(gameTime);

            base.Update(gameTime);
        }

        private void falling()
        {
            this.velocity.Y += 0.98f; // força para pulo é aumentada; 0.98, para ser equivalente a gravidade da Terra.      
        }


        public void verifyGround(Rectangle rectangle)
        {
            // se tiver chegado na posição do chão
            if (animations.position.Y >= rectangle.Y - this.animations.GetAnimation().rectangle.Height && (between((int)animations.position.X, (rectangle.X - 16), (rectangle.X + 16))))
                {
                    isFalling = false;
                    stopToFall = rectangle;
                    stopToFall.Y = rectangle.Y - this.animations.GetAnimation().rectangle.Height;
                }
                else
                    if ((between((int)animations.position.X, (rectangle.X - 16), (rectangle.X + 16))))// && animations.position.Y < Game.Window.ClientBounds.Height - 32)
                    {
                        isFalling = true;
                        stopToFall.Y = Game.Window.ClientBounds.Height + 64;
                    }
        }

        public bool dieByTouch(int positionX, int positionY) { 
            return ((between((int)animations.position.X, positionX - 24, positionX + 24)) && (between((int)animations.position.Y, positionY - 24, positionY + 24)));
        }

        public bool walkOnScreen() {
            return (animations.position.X < Window.ClientBounds.Width / 2);
        }

        private bool haveTile(Rectangle rectangle) {
            return(between((int)animations.position.X, (rectangle.X - 16), (rectangle.X + 16)) && rectangle.Y - 64 >= animations.position.Y && rectangle.Y < Window.ClientBounds.Height - 32);
        }

        private bool nearTile() {
            return (between((int)this.animations.position.Y, Convert.ToInt32(stopToFall.Y), Convert.ToInt32(stopToFall.Y + 64))
                && (between((int)animations.position.X, (stopToFall.X - 16), (stopToFall.X + 16))));
        }

        private bool between(int p, int p1, int p2) {
            return p1 <= p && p <= p2;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            animations.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime); // desenha
        }

    }
}
