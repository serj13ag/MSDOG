using System;
using Core.Sounds;

namespace Core.Models.Data
{
    [Serializable]
    public class SoundEffectData
    {
        public SfxType Type;
        public SoundClip AudioClip;
    }
}