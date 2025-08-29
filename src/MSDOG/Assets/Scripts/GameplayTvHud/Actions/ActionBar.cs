using GameplayTvHud.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayTvHud.Actions
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private AlarmIcon _alarmIcon;
        [SerializeField] private float _firstThreshold = 0.4f;
        [SerializeField] private float _secondThreshold = 0.8f;
        [SerializeField] private bool _invertColoring;

        public void ActivateAlarm()
        {
            _alarmIcon.ActivateAlarm();
        }

        public void DeactivateAlarm()
        {
            _alarmIcon.DeactivateAlarm();
        }

        public void UpdateView(float value)
        {
            Color color;
            if (value < _firstThreshold)
            {
                color = _invertColoring ? Color.red : Color.green;
            }
            else if (value < _secondThreshold)
            {
                color = Color.yellow;
            }
            else
            {
                color = _invertColoring ? Color.green : Color.red;
            }

            _fillImage.fillAmount = value;
            _fillImage.color = color;
        }
    }
}