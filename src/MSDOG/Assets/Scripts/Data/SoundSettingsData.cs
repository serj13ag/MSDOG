using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "SoundSettingsData", menuName = "Data/SoundSettingsData")]
    public class SoundSettingsData : ScriptableObject
    {
        public List<SoundEffectData> Effects;
        public AudioClip MenuMusic;
    }
}