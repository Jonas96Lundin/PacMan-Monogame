using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace PacMan
{
    class Level
    {
        protected static Tile[,] tileArray;
        public static int tileSize = 32;
        protected Rectangle posRec, sourceRect;
        protected List<string> strings;
        protected Point numberOfFrames, currentFrame, fruitFrame;
        protected Tile.Tiles tileType;
        protected bool wall, pellet, fruit, powerUp;
        public static int stringLength, stringCount, pelletCounter;
        List<Enemy> ghosts;
        List<PowerUp> powerUps;
        Player player;

        public Level(string fileName)
        {
            numberOfFrames = new Point(4, 4);
            currentFrame = new Point(0, 0);
            sourceRect = new Rectangle(currentFrame.X * TextureManager.texTileset.Width / numberOfFrames.X,
                                currentFrame.Y * TextureManager.texTileset.Height / numberOfFrames.Y, (TextureManager.texTileset.Width / numberOfFrames.X) - 1,
                                (TextureManager.texTileset.Height / numberOfFrames.Y) - 1);
            stringLength = 0;
            stringCount = 0;
            CreateLevel(fileName);

        }
        public void CreateLevel(string fileName)
        {
            strings = ReadFromFile(fileName);
            tileArray = new Tile[strings[0].Length, strings.Count];
            ghosts = new List<Enemy>();
            powerUps = new List<PowerUp>();
            int ghostTypeCounter = 0;
            for (int i = 0; i < strings.Count; i++)
            {
                for (int j = 0; j < strings[i].Length; j++)
                {
                    posRec = new Rectangle(j * tileSize, i * tileSize, tileSize, tileSize);

                    if (strings[i][j] == '-' || strings[i][j] == 'P' || strings[i][j] == 'G')
                    {
                        tileType = Tile.Tiles.empty;
                        wall = false;
                        pellet = false;
                        fruit = false;
                        powerUp = false;
                        if (strings[i][j] == 'G')
                        {
                            ghostTypeCounter++;
                            Enemy ghost = new Enemy(new Vector2(j * tileSize, i * tileSize), new Vector2(0, 0), 112f, new Point(8, 7), new Point(0, 0), ghostTypeCounter);
                            ghosts.Add(ghost);
                        }
                        if (strings[i][j] == 'P')
                        {
                            player = new Player(new Vector2(j * tileSize, i * tileSize), new Vector2(0, 0), 128f, new Point(4, 1), new Point(0, 0));
                        }
                    }
                    else if (strings[i][j] == '*')
                    {
                        tileType = Tile.Tiles.pellet;
                        wall = false;
                        pellet = true;
                        fruit = false;
                        powerUp = false;
                        pelletCounter++;
                    }
                    else if (strings[i][j] == 'F')
                    {
                        tileType = Tile.Tiles.fruit;
                        wall = false;
                        pellet = false;
                        fruit = true;
                        powerUp = false;
                        numberOfFrames = new Point(4, 1);

                    }
                    else if (strings[i][j] == 'E')
                    {
                        tileType = Tile.Tiles.powerUp;
                        wall = false;
                        pellet = false;
                        fruit = false;
                        powerUp = true;
                        PowerUp power = new PowerUp(new Vector2(posRec.X, posRec.Y));
                        powerUps.Add(power);
                    }
                    else if (strings[i][j] == 'w')
                    {
                        tileType = Tile.Tiles.wall;
                        wall = true;
                        pellet = false;
                        fruit = false;
                        powerUp = false;
                        numberOfFrames = new Point(4, 4);
                        // Wall below
                        if (strings[i + 1][j] == 'w')
                        {   // Wall to the left
                            if (strings[i][j - 1] == 'w')
                            {
                                currentFrame.Y = 3;
                            }
                            else
                            {
                                currentFrame.Y = 2;
                            }
                        }
                        // No wall below
                        else
                        {   // Wall to the left
                            if (strings[i][j - 1] == 'w')
                            {
                                currentFrame.Y = 1;
                            }
                            else
                            {
                                currentFrame.Y = 0;
                            }
                        }
                        // Wall above
                        if (strings[i - 1][j] == 'w')
                        {   // Wall to the right
                            if (strings[i][j + 1] == 'w')
                            {
                                currentFrame.X = 3;
                            }
                            else
                            {
                                currentFrame.X = 2;
                            }
                        }
                        // No wall above
                        else
                        {
                            // Wall to the right
                            if (strings[i][j + 1] == 'w')
                            {
                                currentFrame.X = 1;
                            }
                            else
                            {
                                currentFrame.X = 0;
                            }
                        }
                    }
                    if (pellet)
                    {
                        sourceRect = new Rectangle(0, 0, TextureManager.texPellet.Width, TextureManager.texPellet.Height);
                    }
                    else if (fruit)
                    {
                        if (fruitFrame.X > 3)
                        {
                            fruitFrame.X = 0;
                        }
                        currentFrame.X = fruitFrame.X;
                        currentFrame.Y = 0;
                        fruitFrame.X++;
                        sourceRect = new Rectangle((currentFrame.X * TextureManager.texFruits.Width / numberOfFrames.X),
                                    (currentFrame.Y * TextureManager.texFruits.Height / numberOfFrames.Y), (TextureManager.texFruits.Width / numberOfFrames.X),
                                    (TextureManager.texFruits.Height / numberOfFrames.Y));
                    }
                    else if (wall)
                    {
                        sourceRect = new Rectangle((currentFrame.X * TextureManager.texTileset.Width / numberOfFrames.X),
                                    (currentFrame.Y * TextureManager.texTileset.Height / numberOfFrames.Y), (TextureManager.texTileset.Width / numberOfFrames.X),
                                    (TextureManager.texTileset.Height / numberOfFrames.Y));

                    }
                    else
                    {
                        sourceRect = new Rectangle(0, 0, TextureManager.texPellet.Width, TextureManager.texPellet.Height);
                    }

                    tileArray[j, i] = new Tile(posRec, sourceRect, tileType, wall, pellet, fruit, powerUp);
                }
            }
            stringLength = strings[0].Length;
            stringCount = strings.Count;
        }
        public List<string> ReadFromFile(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            List<string> strings = new List<string>();

            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                strings.Add(s);
            }
            sr.Close();
            return strings;
        }
        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            foreach (Tile tile in tileArray)
            {
                tile.Draw(sb, gameTime);
            }
        }
        // Check if it is a wall at given position
        public static bool GetTileAtPosition(Vector2 vec)
        {
            return tileArray[(int)vec.X / tileSize, (int)vec.Y / tileSize].wall;
        }

        //---------------------------- Pellets ---------------------------
        // Check if it is a pellet at given position
        public static bool CheckPelletAtPosition(Vector2 vec)
        {
            return tileArray[(int)vec.X / tileSize, (int)vec.Y / tileSize].pellet;
        }
        public static bool SetPelletAtPosition(Vector2 vec)
        {
            pelletCounter--;
            return tileArray[(int)vec.X / tileSize, (int)vec.Y / tileSize].pellet = false;
        }
        // Converts pellets to empty tiles
        public void CheckEatables()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                for (int j = 0; j < strings[i].Length; j++)
                {
                    if (strings[i][j] == '*' && !tileArray[j, i].pellet && tileArray[j, i].GetTileType() == Tile.Tiles.pellet)
                    {
                        tileType = Tile.Tiles.empty;
                        tileArray[j, i] = new Tile(posRec, sourceRect, tileType, false, false, false, false);
                        Game1.score += 10;
                    }
                    if (strings[i][j] == 'F' && !tileArray[j, i].fruit && tileArray[j, i].GetTileType() == Tile.Tiles.fruit)
                    {
                        tileType = Tile.Tiles.empty;
                        tileArray[j, i] = new Tile(posRec, sourceRect, tileType, false, false, false, false);
                        Game1.score += 150;
                    }
                    if (strings[i][j] == 'E' && !tileArray[j, i].powerUp && tileArray[j, i].GetTileType() == Tile.Tiles.powerUp)
                    {
                        for(int k = 0; k < ghosts.Count; k++)
                        {
                            ghosts[k].SetScared(true);
                        }
                        tileType = Tile.Tiles.empty;
                        tileArray[j, i] = new Tile(posRec, sourceRect, tileType, false, false, false, false);
                    }
                }
            }
        }
        public int GetPelletAmount()
        {
            return pelletCounter;
        }
        public void SetPelletAmount()
        {
            int pelletAmount = 0;
            for (int i = 0; i < strings.Count; i++)
            {
                for (int j = 0; j < strings[i].Length; j++)
                {
                    if (strings[i][j] == '*')
                    {
                        pelletAmount++;
                    }
                }
            }
            pelletCounter = pelletAmount;
        }

        //------------------------- Fruits ---------------------------
        //public void CheckFruits()
        //{
        //    for (int i = 0; i < strings.Count; i++)
        //    {
        //        for (int j = 0; j < strings[i].Length; j++)
        //        {
        //            if (strings[i][j] == 'F' && !tileArray[j, i].fruit && tileArray[j, i].GetTileType() == Tile.Tiles.fruit)
        //            {
        //                tileType = Tile.Tiles.empty;
        //                tileArray[j, i] = new Tile(posRec, sourceRect, tileType, false, false, false);
        //                Game1.score += 150;
        //            }
        //        }
        //    }
        //}
        // Check if it is a fruit at given position
        public static bool CheckFruitAtPosition(Vector2 vec)
        {
            return tileArray[(int)vec.X / tileSize, (int)vec.Y / tileSize].fruit;
        }
        public static bool SetFruitAtPosition(Vector2 vec)
        {
            return tileArray[(int)vec.X / tileSize, (int)vec.Y / tileSize].fruit = false;
        }
        public static bool CheckPowerUpAtPosition(Vector2 vec)
        {
            return tileArray[(int)vec.X / tileSize, (int)vec.Y / tileSize].powerUp;
        }
        public static bool SetPowerUpAtPosition(Vector2 vec)
        {
            return tileArray[(int)vec.X / tileSize, (int)vec.Y / tileSize].powerUp = false;
        }

        public List<Enemy> GetGhostList()
        {
            return ghosts;
        }
        public Player GetPlayer()
        {
            return player;
        }
        public List<PowerUp> GetPowerUpList()
        {
            return powerUps;
        }
    }
}