using Core.Models.Data;
using Core.Sounds;
using UnityEngine;

namespace Core.Controllers
{
    public interface ISoundController
    {
        void PlayMusic(AudioClip musicClip);
        void PlaySfx(SfxType type);
        void PlayAbilityActivationSfx(SoundClip abilityActivationSoundClip);
    }
}