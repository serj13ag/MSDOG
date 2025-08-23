using Gameplay;
using GameplayTvHud.DetailsZone;
using UnityEngine;

namespace GameplayTvHud.Factories
{
    public interface IDetailViewFactory
    {
        DetailPartHud CreateDetailPartView(Detail detail, Transform parentTransform, Canvas parentCanvas);
    }
}