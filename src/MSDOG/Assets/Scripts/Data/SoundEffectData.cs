using System;
using Sounds;

namespace Data
{
    [Serializable]
    public class SoundEffectData
    {
        public SfxType Type;
        public SoundClip AudioClip;
    }
}