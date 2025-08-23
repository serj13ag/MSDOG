using UnityEngine;

namespace Utility
{
    public static class ColliderHelper
    {
        // TODO: not only collider helper
        public static bool TryGetComponentInHierarchy<T>(this GameObject gameObject, out T component)
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