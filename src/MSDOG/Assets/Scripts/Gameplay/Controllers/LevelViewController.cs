using Core.Services;
using UnityEngine;
using VContainer;

namespace Gameplay.Controllers
{
    public class LevelViewController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _floorMeshRenderer;

        private IDataService _dataService;

        [Inject]
        public void Construct(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void InitLevel(int levelIndex)
        {
            var levelData = _dataService.GetLevelData(levelIndex);
            _floorMeshRenderer.material = levelData.FloorMaterial;
        }
    }
}