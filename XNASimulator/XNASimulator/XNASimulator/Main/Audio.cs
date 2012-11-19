using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNASimulator
{
    class Audio
    {
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;
        SoundEffect soundEffect;

        public Audio(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Content");
        }

        public void PlayBackgroundMusic()
        {
            soundEffect = Content.Load<SoundEffect>("Audio/Music/backgroundmusic");
            soundEffect.Play();
        }
    }
}
