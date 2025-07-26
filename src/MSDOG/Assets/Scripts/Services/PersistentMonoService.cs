using UnityEngine;

namespace Services
{
    public abstract class PersistentMonoService : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}