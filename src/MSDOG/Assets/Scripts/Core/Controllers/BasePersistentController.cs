using UnityEngine;

namespace Core.Controllers
{
    public abstract class BasePersistentController : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}