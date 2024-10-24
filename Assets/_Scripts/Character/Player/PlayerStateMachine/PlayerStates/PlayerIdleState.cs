using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerIdleState : PlayerState
    {
        private readonly CharacterIdleAnimation _idleAnimation = new();
        private readonly CharacterStateMachine _characterStateMachine;

        public PlayerIdleState(PlayerManager player, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(player, stateMachine, eventBus)
        {
            _characterStateMachine = stateMachine;
        }

        public override void EnterState()
        {
            base.EnterState();

            _idleAnimation.SetTags(_player.MainDirection.ToString(), _player.SecDirection.ToString());
            _player.PlayerAnimationManager.PlayAnimation(_idleAnimation);

            if (_player.IsOwner)
                _player.CharacterAttackManager.OnAttackStart += EnterAttackStartState;
        }

        private void EnterAttackStartState()
        {
            _characterStateMachine.ChangeStateRPC((int)CharacterStateMachine.CharacterStates.AttackState);
        }


        public override void ExitState()
        {
            base.ExitState();
            if (_player.IsOwner)
                _player.CharacterAttackManager.OnAttackStart -= EnterAttackStartState;
        }

        public override void FrameUpdate()
        {
            if (!_player.IsOwner) return;

            _player.PlayerMovementManager.HandleAllMovement();


            if (_player.PlayerMovementManager.IsMoving && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC((int)CharacterStateMachine.CharacterStates.MovementState);
            }
        }

        public override void PhysicsUpdate()
        {
            if (!_player.IsOwner) return;

            _player.PlayerMovementManager.HandleAllMovement();
        }
    }
}