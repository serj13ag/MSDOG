using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "SoundSettingsData", menuName = "Data/SoundSettingsData")]
    public class SoundSettingsData : ScriptableObject
    {
        public AudioClip MenuMusic;
    }
}