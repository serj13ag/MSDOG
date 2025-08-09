using Constants;
using Core.Controllers;
using Core.Services;
using Gameplay.Providers;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.Factories
{
    public class ExperiencePieceFactory : IExperiencePieceFactory
    {
        private const int NumberOfPrewarmedPrefabs = 20;

        private readonly IUpdateController _updateController;

        private readonly ObjectPool<ExperiencePiece> _pool;

        public ExperiencePieceFactory(IAssetProviderService assetProviderService, IUpdateController updateController,
            IObjectContainerProvider objectContainerProvider)
        {
            _updateController = updateController;

            _pool = new ObjectPool<ExperiencePiece>(
                createFunc: () => assetProviderService.Instantiate<ExperiencePiece>(AssetPaths.ExperiencePiecePrefab,
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
            experiencePiece.Init(position, experience, _updateController);
            return experiencePiece;
        }
    }
}