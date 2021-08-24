using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    class Highscore
    {
        List<int> strings;
        StreamReader sr;
        StreamWriter sw;
        public Highscore(string fileName)
        {
            strings = new List<int>();
            ReadFromFile(fileName);
        }
        public void ReadFromFile(string fileName)
        {
            sr = new StreamReader(fileName);

            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                strings.Add((Int32.Parse(s)));
                Console.WriteLine(s);
            }
            sr.Close();
        }
        public void WriteToFile(string fileName)
        {
            sw = new StreamWriter(fileName/*, append: true*/);
            //for (int i = 0; i < strings.Count; i++)
            //{
            //    if (Game1.score > strings[i])
            //    {
            //        sw.WriteLine(Game1.score.ToString());
            //        strings.Add(Game1.score);
            //        Console.WriteLine(Game1.score);
            //        break;
            //    }
            //}
            for (int i = 0; i < strings.Count; i++)
            {
                if (Game1.score > strings[i])
                {
                    // Saves 5 highscores
                    if (strings.Count < 5)
                    {
                        strings.Add(Game1.score);
                    }
                    else
                    {
                        strings.RemoveAt(strings.Count - 1);
                        strings.Add(Game1.score);
                    }
                    Console.WriteLine(Game1.score);
                    break;
                }
            }
            strings.Sort();
            strings.Reverse();
            for (int i = 0; i < strings.Count; i++)
            {
                sw.WriteLine(strings[i]);
            }
            //if (Int32.Parse(strings[strings.Count - 1]) < (int)Game1.score)
            //{
            //    sw.WriteLine(Game1.score.ToString());
            //    strings.Add(Game1.score.ToString());
            //    Console.WriteLine(Game1.score);
            //}
            sw.Close();
        }
        public void DrawStart(SpriteBatch spriteBatch)
        {
            Vector2 size = TextureManager.fontHUD.MeasureString("----- Current Highscore: " + strings[0] + " -----");
            spriteBatch.DrawString(TextureManager.fontHUD, "----- Current Highscore: " + strings[0] + " -----", new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, 400), Color.Orange);
        }
        public void ShowHighscoreList(SpriteBatch spriteBatch)
        {
            Vector2 size = TextureManager.fontHUD.MeasureString("Back to menu");
            spriteBatch.DrawString(TextureManager.fontHUD, "Back to menu", new Vector2(150, 0), Color.White);
            int counter = 0;
            size = TextureManager.fontHUD.MeasureString("------- HighScores -------");
            spriteBatch.DrawString(TextureManager.fontHUD, "------- HighScores -------", new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, 100), Color.Orange);
            for (int i = 0; i <= strings.Count - 1; i++)
            {
                size = TextureManager.fontHUD.MeasureString(strings[i].ToString());
                spriteBatch.DrawString(TextureManager.fontHUD, strings[i].ToString(), new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, 200 + 100 * counter), Color.White);
                size = TextureManager.fontHUD.MeasureString("---------------");
                spriteBatch.DrawString(TextureManager.fontHUD, "---------------", new Vector2(Game1.gameBorder.Width / 2 - size.X / 2, 250 + 100 * counter - 1), Color.White);
                counter++;
            }
        }
    }
}
