using UnityEngine;

namespace Services
{
    public class AssetProviderService
    {
        public T GetAsset<T>(string path) where T : Object
        {
            return LoadAsset<T>(path);
        }

        public T Instantiate<T>(string path) where T : Object
        {
            var prefab = LoadAsset<T>(path);
            var instance = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            instance.name = prefab.name;

            return instance;
        }

        public T Instantiate<T>(string path, Transform parentTransform) where T : Object
        {
            var prefab = LoadAsset<T>(path);
            var instance = Object.Instantiate(prefab, parentTransform);
            instance.name = prefab.name;

            return instance;
        }

        public T Instantiate<T>(string path, Vector3 position) where T : Object
        {
            var prefab = LoadAsset<T>(path);
            var instance = Object.Instantiate(prefab, position, Quaternion.identity);
            instance.name = prefab.name;

            return instance;
        }

        private static T LoadAsset<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }
    }
}