using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PacMan
{
    class GameManager
    {
        enum GameState { menu, start, highscore, playing, end, win, lose };
        GameState currentGameState;
        Level level;
        Highscore highscore;
        MenuPointer pointer;
        public static int levelCounter;
        public GameManager()
        {
            levelCounter = 1;
        }
        public void Initialize()
        {
            currentGameState = GameState.menu;
            level = new Level("Level_1.txt");
            highscore = new Highscore("HighscoreList.txt");
            pointer = new MenuPointer();
        }
        public void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();
            switch (currentGameState)
            {
                case GameState.menu:
                    pointer.Update();
                    if ((KeyMouseReader.KeyPressedOnce(Keys.Enter) || KeyMouseReader.GamePadPressedOnce(Buttons.A)) && !pointer.GetPointerStatus())
                    {
                        currentGameState = GameState.playing;
                    }
                    if ((KeyMouseReader.KeyPressedOnce(Keys.Enter) || KeyMouseReader.GamePadPressedOnce(Buttons.A)) && pointer.GetPointerStatus())
                    {
                        currentGameState = GameState.highscore;
                    }
                    break;
                case GameState.playing:
                    if (!level.GetPlayer().GetPlayerIsDead())
                    {
                        level.GetPlayer().Move(gameTime);
                        level.GetPlayer().Eat();
                        level.GetPlayer().Animate(gameTime);
                        level.GetPlayer().ChooseDirection();

                        level.CheckEatables();
                        //level.CheckFruits();
                        // Losing all lives
                        if (level.GetPlayer().GetPlayerHealth() <= 0)
                        {
                            currentGameState = GameState.lose;
                        }
                        // Winning by eating all pellets
                        if (level.GetPelletAmount() <= 0)
                        {
                            currentGameState = GameState.win;
                        }

                        for (int i = 0; i < level.GetGhostList().Count; i++)
                        {
                            if (i == 0)
                            {
                                level.GetGhostList()[i].FollowPlayer(gameTime, level.GetPlayer());
                            }
                            else if (i == 1)
                            {
                                level.GetGhostList()[i].RandomDirection(gameTime, level.GetPlayer());
                            }
                            else if (i == 2)
                            {
                                level.GetGhostList()[i].RandomDirectionAfterWall(gameTime, level.GetPlayer());
                            }
                            else if (i == 3)
                            {
                                level.GetGhostList()[i].RandomDirectionAfterWall(gameTime, level.GetPlayer());
                            }
                            level.GetGhostList()[i].Move(gameTime);
                            level.GetGhostList()[i].Animate(gameTime);

                            if (level.GetPlayer().Collide(level.GetGhostList()[i].GetBoundingBox()) && !level.GetGhostList()[i].GetIsScared())
                            {
                                foreach (Enemy ghost in level.GetGhostList())
                                {
                                    ghost.ResetCharacterPos();
                                }
                                level.GetPlayer().PlayerLoseLife();
                            }
                            else if(level.GetPlayer().Collide(level.GetGhostList()[i].GetBoundingBox()) && level.GetGhostList()[i].GetIsScared())
                            {
                                level.GetGhostList()[i].ResetCharacterPos();
                                level.GetGhostList()[i].SetScared(false);
                                level.GetGhostList()[i].SetColor(Color.White);
                            }

                        }
                        for (int i = 0; i < level.GetPowerUpList().Count; i++)
                        {
                            if (level.GetPlayer().Collide(level.GetPowerUpList()[i].GetBoundingBox()))
                            {
                                level.GetPowerUpList()[i].SetPickedUp();
                            }
                        }
                    }
                    break;
                case GameState.win:
                    if (KeyMouseReader.KeyPressedOnce(Keys.Enter) || KeyMouseReader.GamePadPressedOnce(Buttons.A))
                    {
                        if (levelCounter == 1)
                        {
                            int currentHealth = level.GetPlayer().GetPlayerHealth();
                            level = new Level("level_2.txt");
                            Debug.WriteLine(currentHealth);
                            levelCounter++;
                            currentGameState = GameState.playing;
                            level.SetPelletAmount();
                            level.GetPlayer().SetPlayerHealth(currentHealth);
                            Debug.WriteLine(level.GetPlayer().GetPlayerHealth());
                        }
                        else if (levelCounter == 2)
                        {
                            int currentHealth = level.GetPlayer().GetPlayerHealth();
                            level = new Level("level_3.txt");
                            levelCounter++;
                            currentGameState = GameState.playing;
                            level.SetPelletAmount();
                            level.GetPlayer().SetPlayerHealth(currentHealth);
                        }
                        else if (levelCounter == 3)
                        {
                            int currentHealth = level.GetPlayer().GetPlayerHealth();
                            level = new Level("level_1.txt");
                            levelCounter = 1;
                            currentGameState = GameState.playing;
                            level.SetPelletAmount();
                            level.GetPlayer().SetPlayerHealth(currentHealth);
                        }
                    }
                    break;
                case GameState.lose:
                    if (KeyMouseReader.KeyPressedOnce(Keys.Enter) || KeyMouseReader.GamePadPressedOnce(Buttons.A))
                    {
                        highscore.WriteToFile("HighscoreList.txt");
                        ResetGame();
                    }
                    break;
                case GameState.highscore:
                    pointer.Update();
                    if (KeyMouseReader.KeyPressedOnce(Keys.Enter) || KeyMouseReader.GamePadPressedOnce(Buttons.A) || KeyMouseReader.GamePadPressedOnce(Buttons.B))
                    {
                        currentGameState = GameState.menu;
                    }
                    break;
            }
        }
        private void ResetGame()
        {
            currentGameState = GameState.menu;
            level = new Level("level_1.txt");
            levelCounter = 1;
            Game1.score = 0;
            level.SetPelletAmount();
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.menu:
                    DrawStartScreen(spriteBatch);
                    break;
                case GameState.playing:
                    level.Draw(spriteBatch, gameTime);
                    DrawHUD(spriteBatch);
                    level.GetPlayer().Draw(spriteBatch);
                    foreach (Enemy g in level.GetGhostList())
                    {
                        g.Draw(spriteBatch);
                    }
                    if (level.GetPlayer().GetPlayerIsDead())
                    {
                        level.GetPlayer().PlayerDiesAnimation(gameTime);
                    }
                    break;
                case GameState.win:
                    DrawWinningScreen(spriteBatch);
                    break;
                case GameState.lose:
                    DrawLosingScreen(spriteBatch);
                    break;
                case GameState.highscore:
                    highscore.ShowHighscoreList(spriteBatch);
                    pointer.DrawHighscore(spriteBatch);
                    break;
            }
        }
        private void DrawStartScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.texStartScreen, new Vector2(Game1.gameBorder.Width / 2 - TextureManager.texStartScreen.Width / 2, 200), Color.White);
            Vector2 size = TextureManager.fontHUD.MeasureString("Start game");
            spriteBatch.DrawString(TextureManager.fontHUD, "Start game", new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, 600), Color.White);
            size = TextureManager.fontHUD.MeasureString("Highscore List");
            spriteBatch.DrawString(TextureManager.fontHUD, "Highscore List", new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, 500), Color.MonoGameOrange);
            highscore.DrawStart(spriteBatch);
            pointer.Draw(spriteBatch);
        }
        private void DrawLosingScreen(SpriteBatch spriteBatch)
        {
            Vector2 size = TextureManager.fontHUD.MeasureString("You lost all you lives....");
            spriteBatch.DrawString(TextureManager.fontHUD, "You lost all you lives....", new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, 200), Color.Red);
            size = TextureManager.fontHUD.MeasureString("You got " + Game1.score + " score!");
            spriteBatch.DrawString(TextureManager.fontHUD, "You got " + Game1.score + " score!", new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, 300), Color.White);
            size = TextureManager.fontHUD.MeasureString("Press 'Enter' or 'A' to go back to menu");
            spriteBatch.DrawString(TextureManager.fontHUD, "Press 'Enter' or 'A' to go back to menu", new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, 400), Color.White);
        }
        private void DrawWinningScreen(SpriteBatch spriteBatch)
        {
            Vector2 size = TextureManager.fontHUD.MeasureString("Press 'Enter' or 'A' to start next Level!");
            spriteBatch.DrawString(TextureManager.fontHUD, "Press 'Enter' or 'A' to start next Level!", new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, Game1.gameBorder.Height / 2), Color.White);
        }
        public void DrawHUD(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < level.GetPlayer().GetPlayerHealth(); i++)
            {
                spriteBatch.Draw(TextureManager.texPlife, new Vector2(32 + i * (TextureManager.texPlife.Width + 2), 704), Color.White);
            }
            Vector2 size = TextureManager.fontHUD.MeasureString("Score: " + Game1.score);
            spriteBatch.DrawString(TextureManager.fontHUD, "Score: " + Game1.score, new Vector2(Game1.gameBorder.Width - size.X - Level.tileSize, 704), Color.White);
        }
    }
}
