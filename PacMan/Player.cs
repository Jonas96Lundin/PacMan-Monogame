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
    class Player : Characters
    {
        double timeBetweenFrames, timer, timeSinceLastFrame;
        private int health;
        bool playerIsDead;
        public Player(Vector2 pos, Vector2 direction, float speed, Point numberOfFrames, Point currentFrame)
            : base(pos, direction, speed, TextureManager.texPacman, numberOfFrames, currentFrame)
        {
            timer = 0.2f;
            timeBetweenFrames = 0.2f;
            health = 3;
            playerIsDead = false;
        }
        public void Animate(GameTime gameTime)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                timer = timeBetweenFrames;
                currentFrame.X++;
                if (currentFrame.X >= 4)
                {
                    currentFrame.X = 0;
                }
            }
            sourceRect.X = currentFrame.X * tex.Width / numberOfFrames.X;
        }

        public void Eat()
        {
            if (Level.CheckPelletAtPosition(pos))
            {
                Level.SetPelletAtPosition(pos);
            }
            if (Level.CheckFruitAtPosition(pos))
            {
                Level.SetFruitAtPosition(pos);
            }
            if (Level.CheckPowerUpAtPosition(pos))
            {
                Level.SetPowerUpAtPosition(pos);
            }
        }
        public void ChooseDirection()
        {
            if (KeyMouseReader.KeyPressed(Keys.Right) || KeyMouseReader.GamePadPressed(Buttons.LeftThumbstickRight))
            {
                chosenKey = Keys.Right;
                degrees = 0;
                chosenEffect = SpriteEffects.None;
            }
            else if (KeyMouseReader.KeyPressed(Keys.Left) || KeyMouseReader.GamePadPressed(Buttons.LeftThumbstickLeft))
            {
                chosenKey = Keys.Left;
                chosenButton = Buttons.LeftThumbstickLeft;
                degrees = 0;
                chosenEffect = SpriteEffects.FlipHorizontally;
            }
            else if (KeyMouseReader.KeyPressed(Keys.Up) || KeyMouseReader.GamePadPressed(Buttons.LeftThumbstickUp))
            {
                chosenKey = Keys.Up;
                chosenButton = Buttons.LeftThumbstickUp;
                degrees = -90;
                chosenEffect = SpriteEffects.None;
            }
            else if (KeyMouseReader.KeyPressed(Keys.Down) || KeyMouseReader.GamePadPressed(Buttons.LeftThumbstickDown))
            {
                chosenKey = Keys.Down;
                chosenButton = Buttons.LeftThumbstickDown;
                degrees = -90;
                chosenEffect = SpriteEffects.FlipHorizontally;
            }
        }
        public Vector2 GetPlayerPos()
        {
            return pos;
        }
        public int GetPlayerHealth()
        {
            return health;
        }
        public void SetPlayerHealth(int amount)
        {
            health = amount;
        }
        public void PlayerLoseLife()
        {
            playerIsDead = true;
            health--;
            ResetCharacterRotation();
        }
        public void PlayerDiesAnimation(GameTime gameTime)
        {
            if (playerIsDead)
            {
                tex = TextureManager.texPacmanDies;
                numberOfFrames.X = 11;
                sourceRect = new Rectangle((currentFrame.X * tex.Width / numberOfFrames.X),
                    (currentFrame.Y * tex.Height / numberOfFrames.Y), (tex.Width / numberOfFrames.X),
                    (tex.Height / numberOfFrames.Y));
                timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeSinceLastFrame >= 0.2)
                {
                    timeSinceLastFrame = 0;

                    currentFrame.X++;

                    if (currentFrame.X >= 11)
                    {
                        ResetCharacterPos();
                        currentFrame.X = 0;
                        playerIsDead = false;
                        tex = TextureManager.texPacman;
                        numberOfFrames.X = 4;
                        sourceRect = new Rectangle((currentFrame.X * tex.Width / numberOfFrames.X),
                                    (currentFrame.Y * tex.Height / numberOfFrames.Y), (tex.Width / numberOfFrames.X),
                                    (tex.Height / numberOfFrames.Y));
                    }
                }
                sourceRect.X = currentFrame.X * tex.Width / numberOfFrames.X;

            }
        }
        public bool GetPlayerIsDead()
        {
            return playerIsDead;
        }
    }
}
