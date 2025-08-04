using System.Numerics;
using Constants;

namespace Gameplay.Services
{
    public class ArenaService
    {
        private readonly Vector2 _size;

        public Vector2 HalfSize => _size / 2f;

        public ArenaService()
        {
            _size = new Vector2(Settings.Arena.Size, Settings.Arena.Size);
        }
    }
}