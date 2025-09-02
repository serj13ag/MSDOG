using Gameplay.Controllers;
using Gameplay.Interfaces;
using VContainer;

namespace Gameplay.Abilities.View.VFX
{
    public class FollowingAbilityVFX : BaseAbilityVFX, IUpdatable
    {
        private IGameplayUpdateController _updateController;

        private IEntityWithAbilities _entityWithAbilities;

        [Inject]
        public void Construct(IGameplayUpdateController updateController)
        {
            _updateController = updateController;
        }

        public override void OnGet()
        {
            base.OnGet();

            _updateController.Register(this);
        }

        public void Init(IEntityWithAbilities followTarget)
        {
            _entityWithAbilities = followTarget;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_entityWithAbilities == null)
            {
                return;
            }

            transform.position = _entityWithAbilities.GetPosition();
        }

        public void Clear()
        {
            Release();
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            _entityWithAbilities = null;
            _updateController.Remove(this);
        }
    }
}