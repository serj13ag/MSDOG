using System;
using UnityEngine;

namespace Sounds
{
    [Serializable]
    public class SoundClip
    {
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1f;
    }
}