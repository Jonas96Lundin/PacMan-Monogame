using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    class PowerUp
    {
        Vector2 pos;
        Rectangle boundingBox, destRect;
        bool pickedUp;
        public PowerUp(Vector2 pos)
        {
            this.pos = pos;
            destRect = new Rectangle((int)this.pos.X, (int)this.pos.Y, Level.tileSize, Level.tileSize);
            boundingBox = new Rectangle(destRect.X - Level.tileSize / 2, destRect.Y - Level.tileSize / 2, Level.tileSize, Level.tileSize);
            pickedUp = false;
        }
        public Rectangle GetBoundingBox()
        {
            return boundingBox;
        }
        public void SetPickedUp()
        {
            pickedUp = true;
        }
    }
}
