using UnityEngine;

namespace Gameplay.VFX
{
    public class FollowingAbilityEffect : MonoBehaviour
    {
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
        }

        private void Update()
        {
            transform.position = _player.transform.position;
        }

        public void Clear()
        {
            Destroy(gameObject);
        }
    }
}