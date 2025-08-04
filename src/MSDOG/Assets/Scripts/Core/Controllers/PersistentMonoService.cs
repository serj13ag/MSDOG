using UnityEngine;

namespace Core.Controllers
{
    public abstract class PersistentMonoService : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}