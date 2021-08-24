using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace PacMan
{
    class Characters : GameObject
    {
        protected Vector2 direction;
        protected float speed;
        protected Vector2 checkIfWallPos;
        protected Vector2 destination;
        protected bool moving, canChangeDir;
        protected bool moveRight, moveLeft, moveUp, moveDown;
        protected Keys chosenKey;
        protected Buttons chosenButton;
        protected int degrees;
        protected SpriteEffects chosenEffect;
        public Characters(Vector2 pos, Vector2 direction, float speed, Texture2D tex, Point numberOfFrames, Point currentFrame)
            : base(pos, tex, numberOfFrames, currentFrame)
        {
            this.direction = direction;
            this.speed = speed;
            destination = startPos;

            moving = false;
            canChangeDir = true;
            SetMobility(true);
        }
        public void Move(GameTime gameTime)
        {
            
            LookAround();
            if (pos.X % Level.tileSize == 16 && pos.Y % Level.tileSize == 16)
            {

                // Move right
                if (chosenKey == Keys.Right && moveRight)
                {
                    direction = new Vector2(1, 0);
                    checkIfWallPos = new Vector2(Level.tileSize - Level.tileSize/2, 0);

                    ChangeDirection();
                    moving = true;

                    rotation = MathHelper.ToRadians(degrees);
                    currentEffect = chosenEffect;
                }
                // Move left
                else if (chosenKey == Keys.Left && moveLeft)
                {
                    direction = new Vector2(-1, 0);
                    checkIfWallPos = new Vector2(-Level.tileSize + Level.tileSize / 2, 0);

                    ChangeDirection();
                    moving = true;

                    rotation = MathHelper.ToRadians(degrees);
                    currentEffect = chosenEffect;
                }
                // Move down
                else if (chosenKey == Keys.Down && moveDown)
                {
                    direction = new Vector2(0, 1);
                    checkIfWallPos = new Vector2(0, Level.tileSize - Level.tileSize / 2);

                    ChangeDirection();
                    moving = true;

                    rotation = MathHelper.ToRadians(degrees);
                    currentEffect = chosenEffect;
                }
                // Move up
                else if (chosenKey == Keys.Up && moveUp)
                {
                    direction = new Vector2(0, -1);
                    checkIfWallPos = new Vector2(0, -Level.tileSize + Level.tileSize / 2);

                    ChangeDirection();
                    moving = true;

                    rotation = MathHelper.ToRadians(degrees);
                    currentEffect = chosenEffect;
                }
                MoveOutOfLevel();
            }

            if (moving)
            {
                canChangeDir = false;
                SetMobility(false);
                pos += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                destRect.X = (int)pos.X;
                destRect.Y = (int)pos.Y;
                boundingBox.X = (int)pos.X - Level.tileSize / 2;
                boundingBox.Y = (int)pos.Y - Level.tileSize / 2;

                if (Vector2.Distance(pos, destination) <= 1f)
                {
                    pos = new Vector2((float)Math.Round(destination.X), (float)Math.Round(destination.Y));
                    destRect.X = (int)pos.X;
                    destRect.Y = (int)pos.Y;
                    boundingBox.X = (int)pos.X - Level.tileSize / 2;
                    boundingBox.Y = (int)pos.Y - Level.tileSize / 2;
                    ChangeDirection();
                }
                if (Level.GetTileAtPosition(pos + checkIfWallPos + direction))
                {
                    moving = false;
                    canChangeDir = true;
                    pos = destination;
                    destRect.X = (int)pos.X;
                    destRect.Y = (int)pos.Y;
                    boundingBox.X = (int)pos.X - Level.tileSize / 2;
                    boundingBox.Y = (int)pos.Y - Level.tileSize / 2;

                }
            }
        }

        private void ChangeDirection()
        {
            Vector2 newDestination = pos + direction * Level.tileSize;
            if (!Level.GetTileAtPosition(newDestination))
            {
                destination = newDestination;
            }
        }
        public void MoveOutOfLevel()
        {
            // Move out of level to the left
            if (pos.X < Level.tileSize * 2 && direction==new Vector2(-1,0))
            {
                pos = new Vector2(Level.stringLength * Level.tileSize - Level.tileSize * 2 + Level.tileSize / 2, pos.Y);
                destRect.X = (int)pos.X;
                destRect.Y = (int)pos.Y;
                moving = false;
            }
            // Move out of level to the right
            else if (pos.X > Level.stringLength * Level.tileSize - Level.tileSize * 2 && direction == new Vector2(1, 0))
            {
                pos = new Vector2(Level.tileSize + Level.tileSize / 2, pos.Y);
                destRect.X = (int)pos.X;
                destRect.Y = (int)pos.Y;
                moving = false;
            }
            // Move out of level up
            else if (pos.Y < Level.tileSize * 2 && direction == new Vector2(0, -1))
            {
                pos = new Vector2(pos.X, Level.stringCount * Level.tileSize - Level.tileSize * 2 + Level.tileSize / 2);
                destRect.X = (int)pos.X;
                destRect.Y = (int)pos.Y;
                moving = false;
            }
            else if (pos.Y > Level.stringCount * Level.tileSize - Level.tileSize * 2 && direction == new Vector2(0, 1))
            {
                pos = new Vector2(pos.X, Level.tileSize + Level.tileSize / 2);
                destRect.X = (int)pos.X;
                destRect.Y = (int)pos.Y;
                moving = false;
            }
        }
        public void LookAround()
        {
            // Look right
            if (!Level.GetTileAtPosition(pos + direction * 5 + new Vector2(Level.tileSize - tex.Width / numberOfFrames.X / 2, 0)))
            {
                moveRight = true;
            }
            // Look left
            if (!Level.GetTileAtPosition(pos + direction * 5 + new Vector2(-Level.tileSize - tex.Width / numberOfFrames.X / 2, 0)))
            {
                moveLeft = true;
            }
            // Look down
            if (!Level.GetTileAtPosition(pos + direction * 5 + new Vector2(0, Level.tileSize - tex.Height / numberOfFrames.Y / 2)))
            {
                moveDown = true;
            }
            // Look up
            if (!Level.GetTileAtPosition(pos + direction * 5 + new Vector2(0, -Level.tileSize - tex.Height / numberOfFrames.Y / 2)))
            {
                moveUp = true;
            }
        }
        public void SetMobility(bool canMove)
        {
            moveRight = canMove;
            moveLeft = canMove;
            moveUp = canMove;
            moveDown = canMove;
        }
        public bool Collide(Rectangle box)
        {
            if (boundingBox.Intersects(box))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ResetCharacterPos()
        {
            pos = startPos;
            destRect = startDestRect;
            boundingBox = startBoundingBox;
            chosenKey = Keys.None;
            moving = false;
            direction = Vector2.Zero;
            rotation = 0;
            currentEffect = SpriteEffects.FlipHorizontally;
        }
        public void ResetCharacterRotation()
        {
            rotation = 0;
            currentEffect = SpriteEffects.FlipHorizontally;
        }
    }
}
