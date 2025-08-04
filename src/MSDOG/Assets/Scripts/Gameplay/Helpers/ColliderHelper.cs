using UnityEngine;

namespace Gameplay.Helpers
{
    public static class ColliderHelper
    {
        public static bool TryGetComponentInHierarchy<T>(this GameObject gameObject, out T component) where T : MonoBehaviour
        {
            if (gameObject.TryGetComponent(out component))
            {
                return true;
            }

            var componentInParent = gameObject.GetComponentInParent<T>();
            if (componentInParent != null)
            {
                component = componentInParent;
                return true;
            }

            return false;
        }
    }
}