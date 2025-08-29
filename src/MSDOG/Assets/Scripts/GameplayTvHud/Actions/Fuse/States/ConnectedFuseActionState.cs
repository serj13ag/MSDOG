using UnityEngine;

namespace GameplayTvHud.Actions.Fuse.States
{
    public class ConnectedFuseActionState : IFuseActionState
    {
        private readonly FuseActionContext _context;

        private Vector3? _previousPlayerPosition;
        private float _counter;

        public ConnectedFuseActionState(FuseActionContext context)
        {
            _context = context;
        }

        public void OnUpdate(float deltaTime)
        {
            var playerPosition = _context.ActionMediator.GetPlayerPosition();

            if (!_previousPlayerPosition.HasValue)
            {
                _previousPlayerPosition = playerPosition;
                return;
            }

            var fuseAction = _context.FuseAction;

            var passedDistance = Vector3.Distance(playerPosition, _previousPlayerPosition.Value);
            if (passedDistance > 0f)
            {
                var timeToAdd = deltaTime * (_context.ActionMediator.PlayerHasNitro ? fuseAction.NitroMultiplier : 1f);
                _counter += timeToAdd;
            }
            else
            {
                _counter -= deltaTime * fuseAction.ReduceMultiplier;
                _counter = Mathf.Max(_counter, 0f);
            }

            _previousPlayerPosition = playerPosition;

            _context.ActionBar.UpdateView(_counter / fuseAction.CounterToDisconnect);

            if (_counter >= fuseAction.CounterToDisconnect)
            {
                fuseAction.ChangeStateToDisconnected();
            }
        }
    }
}