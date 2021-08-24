using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace PacMan
{
    class SoundManager
    {
        public static SoundEffect sfxTemp, sfxWacka;
        public static SoundEffectInstance soundEffectInstance;
        public static float timer;

        public SoundManager(ContentManager content)
        {
            sfxWacka = content.Load<SoundEffect>("Pacman Waka Waka");
            soundEffectInstance = sfxWacka.CreateInstance();
            timer=0f;
        }
        public static void PlaySFX(SoundEffect sound, GameTime gameTime)
        {
            timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timer < 0)
            {
                timer = 100f;
                soundEffectInstance.Play();
            }
            Debug.WriteLine(timer);
            //if (soundEffectInstance.State == SoundState.Stopped)
            //{
            //    soundEffectInstance.Play();
            //    Debug.WriteLine(soundEffectInstance.State);
            //}
        }
    }
}
