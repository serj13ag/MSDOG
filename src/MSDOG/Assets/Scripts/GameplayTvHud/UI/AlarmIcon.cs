using UnityEngine;
using Utility;

namespace GameplayTvHud.UI
{
    public class AlarmIcon : MonoBehaviour
    {
        [SerializeField] private ImageAnimatorComponent _imageAnimatorComponent;

        public void ActivateAlarm()
        {
            _imageAnimatorComponent.Activate();
        }

        public void DeactivateAlarm()
        {
            _imageAnimatorComponent.Deactivate();
        }
    }
}