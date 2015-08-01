using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ImAlive
{
    interface GameElement
    {
        Game Game { get; }

        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spbatch, GameTime gameTime);
        void UnloadContent();
    }
}
