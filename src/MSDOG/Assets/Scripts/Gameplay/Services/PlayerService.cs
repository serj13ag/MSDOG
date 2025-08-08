namespace Gameplay.Services
{
    public class PlayerService
    {
        private Player _player;

        public Player Player => _player;

        public void RegisterPlayer(Player player)
        {
            _player = player;
        }
    }
}