using Core.Services;
using Gameplay.Providers;
using UnityEngine;
using Utility.Pools;
using VContainer;
using VContainer.Unity;

namespace Gameplay.Factories
{
    public class ExperiencePieceFactory : IExperiencePieceFactory
    {
        private const int NumberOfPrewarmedPrefabs = 20;

        private readonly IDataService _dataService;

        private readonly GameObjectPool<ExperiencePiece> _pool;

        public ExperiencePieceFactory(IObjectResolver container, IDataService dataService,
            IObjectContainerProvider objectContainerProvider)
        {
            var settingsData = dataService.GetSettingsData();

            _pool = new GameObjectPool<ExperiencePiece>(() =>
                container.Instantiate(settingsData.ExperiencePiecePrefab, objectContainerProvider.ExperiencePieceContainer));
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
            experiencePiece.Init(position, experience);
            return experiencePiece;
        }
    }
}