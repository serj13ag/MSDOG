using Gameplay.Enemies.EnemyBehaviour.States;

namespace Gameplay.Enemies.EnemyBehaviour
{
    public class RangeBehaviourStateMachine : BaseBehaviourStateMachine
    {
        private const float SpawnTime = 2f;

        private readonly EnemyBehaviourStateMachineContext _context;

        public RangeBehaviourStateMachine(EnemyBehaviourStateMachineContext context)
        {
            _context = context;

            State = new SpawningEnemyState(this, SpawnTime);
        }

        public override void ChangeStateToPostSpawn()
        {
            ChangeStateToWalking();
        }

        public void ChangeStateToWalking()
        {
            ChangeState(new RangeWalkingToPlayerEnemyState(this, _context));
        }

        public void ChangeStateToShooting()
        {
            ChangeState(new ShootingEnemyState(this, _context));
        }
    }
}