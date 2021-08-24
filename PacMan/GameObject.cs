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
    class GameObject
    {
        protected Vector2 pos, startPos;
        public Vector2 StartPos{get;set;}
        protected Texture2D tex;
        protected Rectangle sourceRect, destRect, boundingBox, startDestRect, startBoundingBox;
        protected Point numberOfFrames, currentFrame;
        protected float rotation;
        protected SpriteEffects currentEffect;
        protected Color color;
        public GameObject(Vector2 pos, Texture2D tex, Point numberOfFrames, Point currentFrame)
        {
            startPos.X = pos.X + Level.tileSize / 2;
            startPos.Y = pos.Y + Level.tileSize / 2;
            this.pos = startPos;
            this.tex = tex;
            startDestRect = new Rectangle((int)this.pos.X, (int)this.pos.Y, Level.tileSize, Level.tileSize);
            destRect = startDestRect;
            this.numberOfFrames = numberOfFrames;
            this.currentFrame = currentFrame;
            sourceRect = new Rectangle((currentFrame.X * tex.Width / numberOfFrames.X),
                                (currentFrame.Y * tex.Height / numberOfFrames.Y), (tex.Width / numberOfFrames.X),
                                (tex.Height / numberOfFrames.Y));
            startBoundingBox = new Rectangle(destRect.X - Level.tileSize / 2, destRect.Y - Level.tileSize / 2, Level.tileSize, Level.tileSize);
            boundingBox = startBoundingBox;
            rotation = 0f;
            currentEffect = SpriteEffects.None;
            color = Color.White;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, destRect, sourceRect, color, rotation, new Vector2(tex.Width / numberOfFrames.X / 2, tex.Height / numberOfFrames.Y / 2), currentEffect, 0);
        }
        public Rectangle GetBoundingBox()
        {
            return boundingBox;
        }
        public void SetColor(Color newColor)
        {
            color = newColor;
        }
    }
}
