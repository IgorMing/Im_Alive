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


namespace ImAlive
{
    class Playing : Microsoft.Xna.Framework.Game
    {
        public Character Player;
        public Monster[] Monster;
        public Background bg;
        MapCollection maps;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameTime gameTime;
        int GameSpeed;

        public Playing()
        {
           // graphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";
           // Initialize();
        }

        public void setContent(ContentManager Content)
        {
            this.Content = Content;
        }

        public void setGraphics(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public void setGameTime(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

        public void setSpriteBatch(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void Initialize()
        {
            this.GameSpeed = 5;
            this.LoadStaticContents();
            InitializeObjects();
            // base.Initialize();
        }

        private void LoadStaticContents()
        {
            Background.Content =
            Floor.Content =
            Tile.Content = Content;
        }

        private void InitializeObjects()
        {
            // Instância e carregamento de informações importantes dos objetos.
            //spritebatch = new SpriteBatch(GraphicsDevice);

            // Carregando PLAYER
            this.Player = new Character(Content);
            this.Player.LoadContent(graphics.GraphicsDevice);
            this.Player.Game = this;

            // Carregando vetor MONSTER
            Monster = new Monster[7];
            int initialPosition = 200;
            for (int i = 0; i < Monster.Length; i++)
            {
                initialPosition += 700;
                this.Monster[i] = new Monster(this.Content,initialPosition);
                this.Monster[i].LoadContent(graphics.GraphicsDevice);
                this.Monster[i].Game = this;
            }

            // Carregando BACKGROUND
            bg = new Background("bg", new Rectangle(0, 0, 1800, 1125), new Rectangle(0, 0, 50, 80));
            bg.Game = this;

            // Carregando MAPS
            this.maps = new MapCollection();
            maps.LoadMaps();
        }

        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < Monster.Length; i++)
                Monster[i].GameSpeed = this.GameSpeed;

            if (EndLevel())
            {
                Player.CanMove = false;
                if (Player.animations.currentAnimation != "Celebrar")
                    Player.animations.ChangeAnimation("Celebrar");
            }

            if (!Player.CanMove)
            {
                bg.CanMove = maps.GetMapList[0].CanMove = false;
                for (int i = 0; i < Monster.Length; i++)
                    Monster[i].CanMove = false;
            }
            else
            {
                bg.CanMove = maps.GetMapList[0].CanMove = true;
                for (int i = 0; i < Monster.Length; i++)
                    Monster[i].CanMove = true;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            bg.Update(gameTime);
            maps.Update(gameTime);
            foreach (CollisionTile tile in maps.GetMapList[0].CollisionTiles)
            {
                Player.verifyGround(tile.rectangle);
            }

            this.Monster[0].Update(gameTime, 1450 - bg.positionX, 1180 - bg.positionX, 400);
            this.Monster[1].Update(gameTime, 1710 - bg.positionX, 1555 - bg.positionX, 305);
            this.Monster[2].Update(gameTime, 2100 - bg.positionX, 1900 - bg.positionX, 400);
            this.Monster[3].Update(gameTime, 3100 - bg.positionX, 2930 - bg.positionX, 400);
            this.Monster[4].Update(gameTime, 3535 - bg.positionX, 3250 - bg.positionX, 400);
            this.Monster[5].Update(gameTime, 4970 - bg.positionX, 4695 - bg.positionX, 400);
            this.Monster[6].Update(gameTime, 8650 - bg.positionX, 8540 - bg.positionX, 400);

            // Faz o Player andar no cenário, no início da fase.
            if (Player.walkOnScreen() || bg.positionX >= 8800)
            {
                bg.movingOnScreen =
                    maps.GetMapList[0].movingOnScreen = false;
                Player.movingOnScreen = true;
                Player.Speed = this.GameSpeed;
                bg.Speed = maps.GetMapList[0].Speed = 0;
            }
            else
            {
                bg.movingOnScreen =
                    maps.GetMapList[0].movingOnScreen = true;
                Player.movingOnScreen = false;
                bg.Speed = maps.GetMapList[0].Speed = this.GameSpeed;
            }

            // Impede o Player de sair da tela nos limites da fase;
            if (bg.positionX < 1 || bg.positionX >= 8800)
                Player.canMoveLeft = true;
            else
                Player.canMoveLeft = false;

            // Verifica se algum dos monstros da fase tocaram no Player;
            for (int i = 0; i < Monster.Length; i++)
                if (Player.dieByTouch(Monster[i].positionX, Monster[i].positionY))
                    Player.isDead = true;

            Player.Update(gameTime);
            base.Update(gameTime);
        }

        public bool EndLevel(){
            return (Player.animations.position.X > 700);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            bg.Draw(spriteBatch, gameTime);
            maps.Draw(spriteBatch);
            spriteBatch.End();
            Player.Draw(gameTime);

            for (int i = 0; i < Monster.Length; i++)
                this.Monster[i].Draw(gameTime);
            
            base.Draw(gameTime);
        }

        public void VoltaJogo()
        {
            this.Initialize();
            Player.isDead = false;
            
        }

    }
}
