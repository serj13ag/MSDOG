using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Services
{
    public class AssetProviderService : IAssetProviderService
    {
        // TODO: remove
        public T GetAsset<T>(string path) where T : Object
        {
            return LoadAsset<T>(path);
        }

        public T Instantiate<T>(string path) where T : Component
        {
            return InstantiateInner<T>(path, Vector3.zero, Quaternion.identity, null);
        }

        public T Instantiate<T>(string path, Transform parentTransform, IObjectResolver container = null) where T : Component
        {
            return InstantiateInner<T>(path, parentTransform, container);
        }

        public T Instantiate<T>(string path, Vector3 position) where T : Component
        {
            return InstantiateInner<T>(path, position, Quaternion.identity, null);
        }

        public T Instantiate<T>(string path, Vector3 position, Quaternion rotation) where T : Component
        {
            return InstantiateInner<T>(path, position, rotation, null);
        }

        private T InstantiateInner<T>(string path, Vector3 position, Quaternion rotation, Transform parent,
            IObjectResolver container = null) where T : Component
        {
            var prefab = LoadAsset<T>(path);
            var instance = container != null
                ? container.Instantiate(prefab, position, rotation, parent)
                : Object.Instantiate(prefab, position, rotation, parent);
            instance.name = prefab.name;

            return instance;
        }

        private T InstantiateInner<T>(string path, Transform parentTransform, IObjectResolver container) where T : Component
        {
            var prefab = LoadAsset<T>(path);

            var instance = container != null
                ? container.Instantiate(prefab, parentTransform)
                : Object.Instantiate(prefab, parentTransform);
            instance.name = prefab.name;

            return instance;
        }

        private static T LoadAsset<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}