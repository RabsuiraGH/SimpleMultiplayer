using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public abstract class PlayerState : CharacterState
    {
        protected readonly PlayerManager _player;

        protected PlayerState(PlayerManager player, CharacterStateMachine playerStateMachine, EventBus eventBus)
            : base(player, playerStateMachine, eventBus)
        {
            _player = player;
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            if (!_player.IsOwner) return;
            CheckDeathState();
        }

        private void CheckDeathState()
        {
            if (_stateMachine.CurrentState == _characterStateMachine.DeathState) return;
            if (_player.CharacterDeathManager.IsDead && !_stateMachine.IsChangingState)
            {
                _stateMachine.ChangeStateRPC(_characterStateMachine.DeathState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}