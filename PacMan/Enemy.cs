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
    class Enemy : Characters
    {
        double timeBetweenMoving, timer, timeBetweenFrames, animationTimer, scaredTimer;
        int rndDir, oldDir, dirFrame;
        int ghostType;
        bool isScared;
        public Enemy(Vector2 pos, Vector2 direction, float speed, Point numberOfFrames, Point currentFrame, int ghostType)
            : base(pos, direction, speed, TextureManager.texSpriteSheet, numberOfFrames, currentFrame)
        {
            timer = 1f;
            timeBetweenMoving = 1f;
            animationTimer = 0.4f;
            timeBetweenFrames = 0.4f;
            scaredTimer = 5f;
            rndDir = 0;
            this.ghostType = ghostType;
            isScared = false;
        }
        public void RandomDirection(GameTime gameTime, Player player)
        {
            LookAround();
            if (!isScared)
            {
                timer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (timer <= 0)
                {
                    timer = timeBetweenMoving;

                    oldDir = rndDir;
                    rndDir = Game1.rand.Next(0, 4);
                    if (oldDir == rndDir)
                    {
                        rndDir = Game1.rand.Next(0, 4);
                    }

                }
            }
            else if (isScared)
            {
                ScaredEnemies(player);
            }
            MoveGhost(gameTime);
        }
        public void RandomDirectionAfterWall(GameTime gameTime, Player player)
        {
            LookAround();
            if (!isScared)
            {

                if (IsInsideBox())
                {
                    RandomDirection(gameTime, player);
                }
                else if (Level.GetTileAtPosition(pos + checkIfWallPos + direction))
                {
                    rndDir = Game1.rand.Next(0, 4);
                }
            }
            else if (isScared)
            {
                ScaredEnemies(player);
            }
            MoveGhost(gameTime);
        }
        public void FollowPlayer(GameTime gameTime, Player player)
        {
            LookAround();
            if (!isScared)
            {
                if (IsInsideBox())
                {
                    RandomDirection(gameTime, player);
                }
                else
                {
                    if (pos.X < player.GetPlayerPos().X && moveRight)
                    {
                        rndDir = 0;
                    }
                    if (pos.X > player.GetPlayerPos().X && moveLeft)
                    {
                        rndDir = 1;
                    }
                    if (pos.Y > player.GetPlayerPos().Y && moveUp)
                    {
                        rndDir = 2;
                    }
                    if (pos.Y < player.GetPlayerPos().Y && moveDown)
                    {
                        rndDir = 3;
                    }

                }
            }
            else if (isScared)
            {
                ScaredEnemies(player);
            }
            MoveGhost(gameTime);
        }
        private void MoveGhost(GameTime gameTime)
        {
            //MoveInsideBox(gameTime);
            switch (rndDir)
            {
                case 0:
                    chosenKey = Keys.Right;
                    if (currentFrame.X != 0)
                    {
                        currentFrame.X = 0;
                    }
                    dirFrame = 0;
                    break;
                case 1:
                    chosenKey = Keys.Left;
                    if (currentFrame.X != 2)
                    {
                        currentFrame.X = 2;
                    }
                    dirFrame = 2;
                    break;
                case 2:
                    chosenKey = Keys.Up;
                    if (currentFrame.X != 4)
                    {
                        currentFrame.X = 4;
                    }
                    dirFrame = 4;
                    break;
                case 3:
                    chosenKey = Keys.Down;
                    if (currentFrame.X != 6)
                    {
                        currentFrame.X = 6;
                    }
                    dirFrame = 6;
                    break;
            }
            ScaredTimer(gameTime);
        }
        public void MoveInsideBox(GameTime gameTime, Player player)
        {
            if (IsInsideBox())
            {
                RandomDirection(gameTime, player);
            }
        }
        public bool IsInsideBox()
        {
            if (pos.Y == startPos.Y && pos.X >= 11 * Level.tileSize && pos.X <= 18 * Level.tileSize)
            {
                return true;
            }

            return false;
        }
        public void Animate(GameTime gameTime)
        {
            if (!isScared)
            {
                animationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                currentFrame.Y = ghostType;
                sourceRect.Y = currentFrame.Y * tex.Height / numberOfFrames.Y;
                if (animationTimer <= 0)
                {
                    animationTimer = timeBetweenFrames;
                    currentFrame.X++;
                    if (currentFrame.X >= dirFrame + 2)
                    {
                        currentFrame.X = dirFrame;
                    }
                }
            }
            else if (isScared)
            {
                currentFrame.Y = 5;
                currentFrame.X = 2;
                if(scaredTimer%2 < 0.5)
                {
                    currentFrame.X = 3;
                }
            }
            sourceRect.Y = currentFrame.Y * tex.Height / numberOfFrames.Y;
            sourceRect.X = currentFrame.X * tex.Width / numberOfFrames.X;
        }
        // TESTING MOVEMENT
        public void ChooseDirection()
        {
            if (KeyMouseReader.KeyPressed(Keys.Right))
            {
                chosenKey = Keys.Right;
                if (currentFrame.X != 0)
                {
                    currentFrame.X = 0;
                }
                dirFrame = 0;
                //animationTimer = 0;
            }
            else if (KeyMouseReader.KeyPressed(Keys.Left))
            {
                chosenKey = Keys.Left;
                if (currentFrame.X != 2)
                {
                    currentFrame.X = 2;
                }
                dirFrame = 2;
                //animationTimer = 0;
            }
            else if (KeyMouseReader.KeyPressed(Keys.Up))
            {
                chosenKey = Keys.Up;
                if (currentFrame.X != 4)
                {
                    currentFrame.X = 4;
                }
                dirFrame = 4;
                //animationTimer = 0;
            }
            else if (KeyMouseReader.KeyPressed(Keys.Down))
            {
                chosenKey = Keys.Down;
                if (currentFrame.X != 6)
                {
                    currentFrame.X = 6;
                }
                dirFrame = 6;
                //animationTimer = 0;
            }
        }
        public void PlayerDies()
        {
            ResetCharacterPos();
        }
        public void ScaredEnemies(Player player)
        {

            if (pos.X >= player.GetPlayerPos().X && moveRight)
            {
                rndDir = 0;
            }
            if (pos.X <= player.GetPlayerPos().X && moveLeft)
            {
                rndDir = 1;
            }
            if (pos.Y <= player.GetPlayerPos().Y && moveUp)
            {
                rndDir = 2;
            }
            if (pos.Y >= player.GetPlayerPos().Y && moveDown)
            {
                rndDir = 3;
            }
        }
        public void SetScared(bool scared)
        {
            isScared = scared;
        }
        public bool GetIsScared()
        {
            return isScared;
        }
        public void ScaredTimer(GameTime gameTime)
        {
            if (isScared)
            {
                Debug.WriteLine(scaredTimer);
                scaredTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (scaredTimer > 2)
                {
                    color = Color.LightBlue;
                }
                if (scaredTimer < 2)
                {
                    color = Color.Orange;
                }
                if (scaredTimer < 1)
                {
                    color = Color.Red;
                }
                if (scaredTimer <= 0)
                {
                    isScared = false;
                    scaredTimer = 5f;
                    color = Color.White;
                }
            }
        }
    }
}
