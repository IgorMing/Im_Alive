using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace ImAlive
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }
        private Song music;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Playing p;
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D loadingScreen;
        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private GameState gameState;
        private Thread backgroundThread;
        private bool isLoading = false;
        MouseState mouseState;
        MouseState previousMouseState;
        int screenWidth = 800, screenHeight = 480;
        float WaitTimeToShowCard = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            ////set the position of the buttons
            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);

            //set the gamestate to start menu
            gameState = GameState.StartMenu;

            //get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            music = Content.Load<Song>("Sounds/ImAlive"); // *********************************************** HABILITAR MUSICA
            MediaPlayer.Play(music);
            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load the buttonimages into the content pipeline
            startButton = Content.Load<Texture2D>(@"start");
            exitButton = Content.Load<Texture2D>(@"exit");

            //load the loading screen
            loadingScreen = Content.Load<Texture2D>(@"loading");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (p != null)
                if (p.EndLevel())
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        VoltarJogo();

            if (gameState == GameState.Loading && !isLoading)
            {
                backgroundThread = new Thread(LoadGame);
                isLoading = true;

                backgroundThread.Start();
                p = new Playing();
                p.setContent(this.Content);
                p.setGraphics(graphics);
                p.setSpriteBatch(this.spriteBatch);
                p.Initialize();

            }

            if (gameState == GameState.Playing)
            {
                MediaPlayer.Volume = 0.4f;
                //Window.Title = Convert.ToString(p.Player.isFalling); // ********************************************** DEBUG ***********
                p.Update(gameTime);
            }

            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed &&
                mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            previousMouseState = mouseState;

            if (gameState == GameState.Playing && isLoading)
            {
                LoadGame();
                isLoading = false;
            }

            if (p != null && p.Player != null)
            {
                if (p.Player.isDead)
                {
                    WaitTimeToShowCard = 1;
                }

                if (WaitTimeToShowCard > 0)
                {
                    VoltarJogo();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // GraphicsDevice.Clear(Color.Black);

            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Content.Load<Texture2D>(@"Images/Menu/MainMenu"), new Rectangle(0, 0, 800, 480), Color.White);
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
                spriteBatch.End();
            }

            //show the loading screen when needed
            if (gameState == GameState.Loading)
            {

                spriteBatch.Begin();
                spriteBatch.Draw(loadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - (loadingScreen.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (loadingScreen.Height / 2)), Color.White);
                spriteBatch.End();
            }

            //draw the the game when playing
            if (gameState == GameState.Playing)
            {
                p.Draw(gameTime);
            }

            //draw the pause screen
            //if (gameState == GameState.Paused)
            //{
            //    spriteBatch.Begin();
            //    spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
            //    spriteBatch.End();
            //}

            base.Draw(gameTime);
        }

        void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                    gameState = GameState.Loading;
                    isLoading = false;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    Exit();
                }
            }

            if (gameState == GameState.Playing)
            {
                Rectangle pauseButtonRect = new Rectangle(0, 0, 70, 70);

                if (mouseClickRect.Intersects(pauseButtonRect))
                {
                    gameState = GameState.Paused;
                }
            }

        }

        void LoadGame()
        {
            Thread.Sleep(3000);

            //start playing
            gameState = GameState.Playing;
            isLoading = false;
        }

        void VoltarJogo()
        {
            WaitTimeToShowCard = 0;
            p.setContent(this.Content);
            p.setGraphics(graphics);
            p.setSpriteBatch(this.spriteBatch);
            p.Initialize();
            p.Player.isDead = false;
        }

    }
}
