using System.Numerics;

namespace Gameplay.Services
{
    public class ArenaService : IArenaService
    {
        private const int Size = 40;

        private readonly Vector2 _size = new(Size, Size);

        public Vector2 HalfSize => _size / 2f;
    }
}