using Gameplay.Enemies.EnemyBehaviour.States;

namespace Gameplay.Enemies.EnemyBehaviour
{
    public class MeleeBehaviourStateMachine : BaseBehaviourStateMachine
    {
        private const float SpawnTime = 1.3f;

        private readonly EnemyBehaviourStateMachineContext _context;

        public MeleeBehaviourStateMachine(EnemyBehaviourStateMachineContext context)
        {
            _context = context;

            State = new SpawningEnemyState(this, SpawnTime);
        }

        public override void ChangeStateToPostSpawn()
        {
            ChangeState(new MeleeWalkingToPlayerEnemyState(this, _context));
        }

        public void ChangeStateToAttacking()
        {
            ChangeState(new AttackingEnemyState(this, _context, 2f));
        }
    }
}