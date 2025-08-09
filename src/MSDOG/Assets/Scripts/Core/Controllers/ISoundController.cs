using Core.Sounds;
using UnityEngine;

namespace Core.Controllers
{
    public interface ISoundController
    {
        void PlayMusic(AudioClip musicClip);
        void StopMusic();
        void PauseMusic();
        void ResumeMusic();

        void PlaySfx(SfxType type);
        void PlayAbilityActivationSfx(SoundClip abilityActivationSoundClip);
    }
}