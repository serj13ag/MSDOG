using Constants;
using UnityEngine;

namespace Services
{
    public class SoundService : BaseMonoService
    {
        [SerializeField] private AudioSource _audioSource;

        private AssetProviderService _assetProviderService;

        public void Init(AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
        }

        public void PlayDestroySound()
        {
            var clip = _assetProviderService.GetAsset<AudioClip>(AssetPaths.DestroySoundPath);
            PlayOneShotClip(clip, 1f);
        }

        private void PlayOneShotClip(AudioClip audioClip, float volume)
        {
            _audioSource.clip = audioClip;
            _audioSource.volume = volume;

            _audioSource.Play();
        }
    }
}