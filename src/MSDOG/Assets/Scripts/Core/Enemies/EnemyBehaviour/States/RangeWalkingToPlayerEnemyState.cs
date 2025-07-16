using Constants;
using UnityEngine;
using UtilityComponents;

namespace Core.Enemies.EnemyBehaviour.States
{
    public class RangeWalkingToPlayerEnemyState : BaseWalkingToPlayerEnemyState
    {
        private readonly RangeBehaviourStateMachine _stateMachine;
        private float _timeTillShoot;

        public RangeWalkingToPlayerEnemyState(RangeBehaviourStateMachine stateMachine, Enemy enemy,
            ColliderEventProvider triggerEnterProvider, float timeTillShoot)
            : base(enemy, triggerEnterProvider)
        {
            _stateMachine = stateMachine;
            _timeTillShoot = timeTillShoot;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (_timeTillShoot > 0f)
            {
                _timeTillShoot -= deltaTime;
            }

            if (Vector3.Distance(Enemy.transform.position, Enemy.Player.transform.position) < Settings.Enemy.RangeCloseDistance)
            {
                Enemy.Agent.ResetPath();
                _stateMachine.ChangeStateToShooting(_timeTillShoot);
            }
        }
    }
}