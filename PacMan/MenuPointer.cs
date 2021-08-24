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
    class MenuPointer
    {
        Rectangle pos;
        Rectangle sourceRect;
        bool isTop, inList;
        GamePadState padState = GamePad.GetState(PlayerIndex.One), oldPadState = GamePad.GetState(PlayerIndex.One);
        public MenuPointer()
        {
            pos = new Rectangle(275 + TextureManager.texPacman.Width / 8, 500 + TextureManager.texPacman.Height / 4, TextureManager.texPacman.Width /4/2, TextureManager.texPacman.Height/2);
            sourceRect = new Rectangle(2 * TextureManager.texPacman.Width / 4, 0, TextureManager.texPacman.Width / 4, TextureManager.texPacman.Height);
            isTop = true;
            inList = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            inList = false;
            spriteBatch.Draw(TextureManager.texPacman, pos,sourceRect, Color.White);
        }
        public void DrawHighscore(SpriteBatch spriteBatch)
        {
            inList = true;
            spriteBatch.Draw(TextureManager.texPacman, pos, sourceRect, Color.White);
        }
        public void Update()
        {
            if (!inList)
            {
                if(pos.Y <= 100)
                {
                    pos = new Rectangle(275 + TextureManager.texPacman.Width / 8, 500 + TextureManager.texPacman.Height / 4, TextureManager.texPacman.Width / 4 / 2, TextureManager.texPacman.Height / 2);
                }
                if ((KeyMouseReader.KeyPressedOnce(Keys.Down) || KeyMouseReader.GamePadPressedOnce(Buttons.LeftThumbstickDown)) && isTop)
                {
                    pos.Y = 600 + TextureManager.texPacman.Height / 4;
                    isTop = false;
                }
                else if ((KeyMouseReader.KeyPressedOnce(Keys.Up) || KeyMouseReader.GamePadPressedOnce(Buttons.LeftThumbstickUp)) && !isTop)
                {
                    pos.Y = 500 + TextureManager.texPacman.Height / 4;
                    isTop = true;
                }
            }
            else if (inList)
            {
                pos = new Rectangle(100 + TextureManager.texPacman.Width / 8, 0 + TextureManager.texPacman.Height / 4, TextureManager.texPacman.Width / 4 / 2, TextureManager.texPacman.Height / 2);
            }
        }
        public bool GetPointerStatus()
        {
            return isTop;
        }
    }

}
