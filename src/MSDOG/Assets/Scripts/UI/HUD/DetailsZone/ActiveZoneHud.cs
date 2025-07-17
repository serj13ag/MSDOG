using System.Collections.Generic;
using Constants;
using Core;
using Data;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.HUD.DetailsZone
{
    public class ActiveZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        private const int MaxNumberOfActiveParts = 4;

        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private GridLayoutGroup _grid;

        private AssetProviderService _assetProviderService;
        private Player _player;

        private readonly List<DetailPartHud> _detailParts = new List<DetailPartHud>(MaxNumberOfActiveParts);

        public void Init(Player player, AssetProviderService assetProviderService)
        {
            _assetProviderService = assetProviderService;
            _player = player;
        }

        public void AddDetail(AbilityData abilityData)
        {
            if (_detailParts.Count >= MaxNumberOfActiveParts)
            {
                Debug.LogError("Too many active detail parts");
                return;
            }

            var detailPart = _assetProviderService.Instantiate<DetailPartHud>(AssetPaths.DetailPartPrefabPath, _grid.transform);
            detailPart.Init(abilityData, _parentCanvas);
            detailPart.SetCurrentZone(this);
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

            detailPart.SetCurrentZone(this);
        }

        public void Enter(DetailPartHud detailPart)
        {
            _detailParts.Add(detailPart);
            detailPart.transform.SetParent(_grid.transform);
            _player.AddAbility(detailPart.Id, detailPart.AbilityData);
        }

        public void Exit(DetailPartHud detailPart)
        {
            _detailParts.Remove(detailPart);
            _player.RemoveAbility(detailPart.Id);
        }
    }
}