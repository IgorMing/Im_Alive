using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ImAlive
{
    class AnimationCollection
    {
        public Dictionary<string, Animation> Animations = new Dictionary<string,Animation>();
        public string currentAnimation;
        public Vector2 position = Vector2.Zero;
        public bool anotherSide {get; set;}

        public AnimationCollection() 
        {
            
        }

        public void AddAnimation(string animationName, Animation animation) 
        {
            Animations.Add(animationName, animation);
        }

        public void ChangeAnimation(string animationName)
        {
            currentAnimation = animationName;
        }

        public Animation GetAnimation()
        {
            return Animations[currentAnimation];
        }

        public void Update(GameTime gameTime)
        {
            Animations[currentAnimation].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            Animations[currentAnimation].Draw(spriteBatch, position, anotherSide);
        }

    }
}
