using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PacMan
{
    class TextureManager
    {
        public static Texture2D texTileset, texTilesetGreen, texTilesetRed, texEmpty, texPacman, texPellet, texSpriteSheet, texStartScreen, texPacmanDies, texPlife, texFruits, texPowerUp;
        public static SpriteFont fontHUD;

        public TextureManager(ContentManager content)
        {
            texTileset = content.Load<Texture2D>("Tileset2");
            texTilesetGreen = content.Load<Texture2D>("Tileset2green");
            texTilesetRed = content.Load<Texture2D>("Tileset2red");
            texEmpty = content.Load<Texture2D>("empty");
            texPacman = content.Load<Texture2D>("pacman_32");
            texPellet = content.Load<Texture2D>("pellet_small");
            texSpriteSheet = content.Load<Texture2D>("SpriteSheetCorrect");
            texStartScreen = content.Load<Texture2D>("PacManStart");
            texPacmanDies = content.Load<Texture2D>("PacmanDies");
            texPlife = content.Load<Texture2D>("pLife");
            texFruits = content.Load<Texture2D>("fruits_narrow");
            texPowerUp = content.Load<Texture2D>("powerUp");
            fontHUD = content.Load<SpriteFont>("HUDfont");
        }
    }
}
