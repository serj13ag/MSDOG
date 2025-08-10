using Core.Services;
using Gameplay.Providers;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class ExperiencePieceFactory : IExperiencePieceFactory
    {
        private const int NumberOfPrewarmedPrefabs = 20;

        private readonly IDataService _dataService;

        private readonly ObjectPool<ExperiencePiece> _pool;

        public ExperiencePieceFactory(IObjectResolver container, IDataService dataService,
            IObjectContainerProvider objectContainerProvider)
        {
            var settingsData = dataService.GetSettingsData();

            _pool = new ObjectPool<ExperiencePiece>(
                createFunc: () => container.Instantiate(settingsData.ExperiencePiecePrefab,
                    objectContainerProvider.ExperiencePieceContainer),
                actionOnGet: obj => obj.OnGet(),
                actionOnRelease: obj => obj.OnRelease());
        }

        public void Prewarm()
        {
            var createdPrefabs = new ExperiencePiece[NumberOfPrewarmedPrefabs];
            for (var i = 0; i < NumberOfPrewarmedPrefabs; i++)
            {
                createdPrefabs[i] = _pool.Get();
            }

            foreach (var prefab in createdPrefabs)
            {
                _pool.Release(prefab);
            }
        }

        public ExperiencePiece CreateExperiencePiece(Vector3 position, int experience)
        {
            var experiencePiece = _pool.Get();
            experiencePiece.SetReleaseCallback(() => _pool.Release(experiencePiece));
            experiencePiece.Init(position, experience);
            return experiencePiece;
        }
    }
}