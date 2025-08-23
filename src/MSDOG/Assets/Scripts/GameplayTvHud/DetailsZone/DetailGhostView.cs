using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayTvHud.DetailsZone
{
    public class DetailGhostView : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void Init(Detail detail)
        {
            _icon.sprite = detail.AbilityData.Icon;
        }
    }
}