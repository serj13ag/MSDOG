using UnityEngine;
using VContainer;

namespace Core.Services
{
    public interface IAssetProviderService
    {
        T GetAsset<T>(string path) where T : Object;
        T Instantiate<T>(string path) where T : Component;
        T Instantiate<T>(string path, Transform parentTransform, IObjectResolver container = null) where T : Component;
        T Instantiate<T>(string path, Vector3 position) where T : Component;
        T Instantiate<T>(string path, Vector3 position, Quaternion rotation) where T : Component;
    }
}