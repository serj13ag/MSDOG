using Core.Controllers;
using Core.Sounds;
using GameplayTvHud.Mediators;
using UnityEngine;
using VContainer;

namespace GameplayTvHud.DetailsZone
{
    public class DestructZoneHud : MonoBehaviour, IDetailsZone, IDetailDropTarget
    {
        private ISoundController _soundController;
        private IDetailMediator _detailMediator;

        [Inject]
        public void Construct(ISoundController soundController, IDetailMediator detailMediator)
        {
            _detailMediator = detailMediator;
            _soundController = soundController;
        }

        public void OnDetailDrop(DetailView detailView)
        {
            detailView.SetCurrentZone(this);
        }

        public void Enter(DetailView detailPart)
        {
            detailPart.Destruct();

            _detailMediator.DetailDestructed();

            _soundController.PlaySfx(SfxType.Destructor);
        }

        public void Exit(DetailView detailPart)
        {
            Debug.LogError("Can't exit from that zone!");
        }
    }
}