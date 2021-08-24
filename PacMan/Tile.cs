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
    class Tile
    {
        public Rectangle pos, sourceRect;
        public bool wall, pellet, fruit, powerUp;
        public enum Tiles { wall, empty, pellet, fruit, powerUp };
        Tiles tileSort;
        string tileType;

        public Tile(Rectangle pos, Rectangle sourceRect, Tiles tileSort, bool wall, bool pellet, bool fruit, bool powerUp)
        {
            Initialize(pos, tileSort, wall, tileType, sourceRect, pellet, fruit, powerUp);

        }

        private void Initialize(Rectangle pos, Tiles tileSort, bool stop, string tileType, Rectangle sourceRect, bool pellet, bool fruit, bool powerUp)
        {
            this.pos = pos;
            this.tileSort = tileSort;
            this.tileType = tileType;
            this.wall = stop;
            this.sourceRect = sourceRect;
            this.pellet = pellet;
            this.fruit = fruit;
            this.powerUp = powerUp;
        }

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            switch (tileSort)
            {
                case Tiles.wall:
                    if (GameManager.levelCounter == 1)
                    {
                        sb.Draw(TextureManager.texTileset, new Rectangle((int)pos.X, (int)pos.Y, Level.tileSize, Level.tileSize), sourceRect, Color.White);
                    }
                    else if(GameManager.levelCounter == 2)
                    {
                        sb.Draw(TextureManager.texTilesetGreen, new Rectangle((int)pos.X, (int)pos.Y, Level.tileSize, Level.tileSize), sourceRect, Color.White);
                    }
                    else if (GameManager.levelCounter == 3)
                    {
                        sb.Draw(TextureManager.texTilesetRed, new Rectangle((int)pos.X, (int)pos.Y, Level.tileSize, Level.tileSize), sourceRect, Color.White);
                    }
                    break;
                case Tiles.empty:
                    sb.Draw(TextureManager.texEmpty, new Rectangle((int)pos.X, (int)pos.Y, Level.tileSize, Level.tileSize), sourceRect, Color.White);
                    break;
                case Tiles.pellet:
                    sb.Draw(TextureManager.texPellet, new Rectangle((int)pos.X, (int)pos.Y, Level.tileSize, Level.tileSize), sourceRect, Color.White);
                    break;
                case Tiles.fruit:
                    sb.Draw(TextureManager.texFruits, new Rectangle((int)pos.X, (int)pos.Y, Level.tileSize, Level.tileSize), sourceRect, Color.White);
                    break;
                case Tiles.powerUp:
                    sb.Draw(TextureManager.texPowerUp, new Rectangle((int)pos.X, (int)pos.Y, Level.tileSize, Level.tileSize), sourceRect, Color.Pink);
                    break;
            }
        }

        public Tiles GetTileType()
        {
            return tileSort;
        }
    }
}