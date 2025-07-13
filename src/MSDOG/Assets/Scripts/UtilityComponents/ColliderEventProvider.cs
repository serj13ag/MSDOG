using System;
using UnityEngine;

namespace UtilityComponents
{
    public class ColliderEventProvider : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEntered;
        public event Action<Collision> OnCollisionEntered;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered?.Invoke(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEntered?.Invoke(other);
        }
    }
}