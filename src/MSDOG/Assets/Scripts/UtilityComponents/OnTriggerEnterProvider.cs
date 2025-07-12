using System;
using UnityEngine;

namespace UtilityComponents
{
    public class OnTriggerEnterProvider : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEntered;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered?.Invoke(other);
        }
    }
}