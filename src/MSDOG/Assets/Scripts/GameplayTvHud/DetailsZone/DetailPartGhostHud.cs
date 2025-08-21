using Core.Models.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameplayTvHud.DetailsZone
{
    public class DetailPartGhostHud : MonoBehaviour
    {
        [SerializeField] private Image _icon;

        public void Init(AbilityData abilityData)
        {
            _icon.sprite = abilityData.Icon;
        }
    }
}