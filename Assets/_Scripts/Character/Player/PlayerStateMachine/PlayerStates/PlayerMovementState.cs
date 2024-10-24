using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerMovementState : PlayerState
    {
        private readonly CharacterWalkAnimation _movingAnimation = new();
        private readonly CharacterStateMachine _characterStateMachine;

        public PlayerMovementState(PlayerManager player, CharacterStateMachine stateMachine, EventBus eventBus)
            : base(player, stateMachine, eventBus)
        {
            _characterStateMachine = stateMachine;
        }

        public override void EnterState()
        {
            base.EnterState();

            _player.OnDirectionChanged += PlayAnimation;
            PlayAnimation(_player.MainDirection, _player.SecDirection);

            if (_player.IsOwner)
                _player.CharacterAttackManager.OnAttackStart += EnterAttackStartState;
        }

        private void PlayAnimation(Directions.MainDirection mainDirection,
                                   Directions.SecondaryDirection secondaryDirection)
        {
            _movingAnimation.SetTags(mainDirection.ToString(), secondaryDirection.ToString());

            _player.PlayerAnimationManager.PlayAnimation(_movingAnimation);
        }

        private void EnterAttackStartState()
        {
            _characterStateMachine.ChangeStateRPC((int)CharacterStateMachine.CharacterStates.AttackState);
        }

        public override void ExitState()
        {
            base.ExitState();

            _player.OnDirectionChanged -= PlayAnimation;

            if (_player.IsOwner)
                _player.CharacterAttackManager.OnAttackStart -= EnterAttackStartState;
        }

        public override void FrameUpdate()
        {
            if (!_player.IsOwner) return;

            if (!_player.PlayerMovementManager.IsMoving && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC((int)CharacterStateMachine.CharacterStates.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            if (!_player.IsOwner) return;

            _player.PlayerMovementManager.HandleAllMovement();
        }
    }
}