using System;
using System.Collections.Generic;
using Constants;
using Core.Models.Data;
using Core.Services;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace UI.HUD.DetailsZone
{
    public class ActiveZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        [SerializeField] private Canvas _parentCanvas;
        [SerializeField] private Transform _grid;
        [SerializeField] private int _maxNumberOfActiveParts;

        private IAssetProviderService _assetProviderService;
        private PlayerService _playerService;

        private readonly Dictionary<Guid, DetailPartHud> _detailParts = new Dictionary<Guid, DetailPartHud>();

        [Inject]
        public void Construct(PlayerService playerService, IAssetProviderService assetProviderService)
        {
            _playerService = playerService;
            _assetProviderService = assetProviderService;
        }

        public void AddDetail(AbilityData abilityData)
        {
            if (_detailParts.Count >= _maxNumberOfActiveParts)
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

            if (_detailParts.ContainsKey(detailPart.Id))
            {
                return;
            }

            if (_detailParts.Count >= _maxNumberOfActiveParts)
            {
                return;
            }

            detailPart.SetCurrentZone(this);
        }

        public void Enter(DetailPartHud detailPart)
        {
            _detailParts.Add(detailPart.Id, detailPart);
            detailPart.transform.SetParent(_grid.transform);
            _playerService.Player.AddAbility(detailPart.Id, detailPart.AbilityData);
        }

        public void Exit(DetailPartHud detailPart)
        {
            _detailParts.Remove(detailPart.Id);
            _playerService.Player.RemoveAbility(detailPart.Id);
        }

        public IEnumerable<DetailPartHud> GetDetailParts()
        {
            foreach (var detailPart in _detailParts)
            {
                yield return detailPart.Value;
            }
        }
    }
}