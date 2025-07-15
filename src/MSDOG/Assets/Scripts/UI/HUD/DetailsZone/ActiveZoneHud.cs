using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.HUD.DetailsZone
{
    public class ActiveZoneHud : MonoBehaviour, IDropHandler
    {
        private const int MaxNumberOfActiveParts = 4;

        [SerializeField] private GridLayoutGroup _grid;

        private Player _player;
        private readonly List<DetailPartHud> _detailParts = new List<DetailPartHud>(MaxNumberOfActiveParts);

        public void Init(Player player)
        {
            _player = player;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            if (!eventData.pointerDrag.gameObject.TryGetComponent<DetailPartHud>(out var detailPart))
            {
                return;
            }

            if (_detailParts.Count >= MaxNumberOfActiveParts)
            {
                return;
            }

            _detailParts.Add(detailPart);
            detailPart.transform.SetParent(_grid.transform);
            _player.AddAbility(detailPart.Detail);
        }
    }
}