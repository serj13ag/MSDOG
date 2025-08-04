using UnityEngine;

namespace Core.Services
{
    public abstract class PersistentMonoService : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}