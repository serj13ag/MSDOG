using Gameplay;
using GameplayTvHud.DetailsZone;
using UnityEngine;

namespace GameplayTvHud.Factories
{
    public interface IDetailViewFactory
    {
        DetailView CreateDetailPartView(Detail detail, Transform parentTransform, Canvas parentCanvas);
        DetailGhostView GetDetailGhost(Detail detail, Transform parentTransform);
    }
}