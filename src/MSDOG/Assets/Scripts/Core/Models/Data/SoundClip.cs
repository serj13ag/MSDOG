using System;
using UnityEngine;

namespace Core.Models.Data
{
    [Serializable]
    public class SoundClip
    {
        public AudioClip Clip;
        [Range(0f, 1f)] public float Volume = 1f;
    }
}