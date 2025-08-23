using UnityEngine;

namespace Core.Controllers
{
    public abstract class BasePersistentController : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}