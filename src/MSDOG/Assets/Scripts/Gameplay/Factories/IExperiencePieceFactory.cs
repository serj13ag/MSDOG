using UnityEngine;

namespace Gameplay.Factories
{
    public interface IExperiencePieceFactory
    {
        void Prewarm();
        ExperiencePiece CreateExperiencePiece(Vector3 position, int experience);
    }
}