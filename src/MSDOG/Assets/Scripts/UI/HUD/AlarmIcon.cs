using UnityEngine;
using UtilityComponents;

namespace UI.HUD
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