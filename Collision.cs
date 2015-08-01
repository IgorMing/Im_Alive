using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ImAlive
{
    static class Collision
    {
        public static bool TopCollision(this Animation player, Rectangle rectangle)
        {
            return (player.rectangle.Bottom >= rectangle.Top - 1 &&
                player.rectangle.Bottom <= rectangle.Top + (rectangle.Height / 2) &&
                player.rectangle.Right >= rectangle.Left + rectangle.Width / 5 &&
                player.rectangle.Left <= rectangle.Right - rectangle.Width / 5);
        }

        public static bool BottomCollision(this Animation player, Rectangle rectangle)
        {
            return (player.rectangle.Top >= rectangle.Bottom + (rectangle.Height / 5) &&
                player.rectangle.Top <= rectangle.Bottom - 1 &&
                player.rectangle.Right >= rectangle.Left + rectangle.Width / 5 &&
                player.rectangle.Left <= rectangle.Right - rectangle.Width /9);
        }

        public static bool LeftCollision(this Animation player, Rectangle rectangle)
        {
            return (player.rectangle.Right <= rectangle.Right &&
                player.rectangle.Right >= rectangle.Left - 5 &&
                player.rectangle.Top <= rectangle.Bottom - (rectangle.Width / 4) &&
                player.rectangle.Bottom >= rectangle.Top + rectangle.Width / 4);
        }

        public static bool RightCollision(this Animation player, Rectangle rectangle)
        {
            return (player.rectangle.Left >= rectangle.Left &&
                player.rectangle.Left <= rectangle.Right - 5 &&
                player.rectangle.Top <= rectangle.Bottom - (rectangle.Width / 4) &&
                player.rectangle.Bottom >= rectangle.Top + rectangle.Width / 4);
        }
    }
}
