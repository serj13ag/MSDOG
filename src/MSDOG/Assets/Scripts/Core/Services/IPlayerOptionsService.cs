using System;

namespace Core.Services
{
    public interface IPlayerOptionsService
    {
        bool IsMuted { get; }
        float MasterVolume { get; }
        float MusicVolume { get; }
        float SfxVolume { get; }

        event EventHandler<EventArgs> OnSoundOptionsUpdated;

        void UpdateOptions(bool isMute, float masterVolume, float musicVolume, float sfxVolume);
    }
}