using UnityEngine;

namespace Services
{
    public abstract class BaseMonoService : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}