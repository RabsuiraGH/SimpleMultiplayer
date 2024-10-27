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


            if (_player.IsOwner)
            {
                _player.OnDirectionChanged += PlayMovementAnimation;
                _player.CharacterAttackManager.OnBasicAttackPerform += EnterBasicAttackPerformState;
                _player.CharacterAttackManager.OnChargeAttackCharge += EnterChargeAttackState;
            }
        }

        private void PlayMovementAnimation(Directions.MainDirection mainDirection,
                                           Directions.SecondaryDirection secondaryDirection)
        {
            _movingAnimation.SetTags(mainDirection.ToString(), secondaryDirection.ToString());

            _player.PlayerAnimationManager.PlayAnimation(_movingAnimation);
        }

        private void EnterChargeAttackState()
        {
            _characterStateMachine.ChangeStateRPC((int)CharacterStateMachine.CharacterStates.ChargeAttackState);
        }

        private void EnterBasicAttackPerformState()
        {
            _characterStateMachine.ChangeStateRPC((int)CharacterStateMachine.CharacterStates.AttackState);
        }

        public override void ExitState()
        {
            base.ExitState();


            if (_player.IsOwner)
            {
                _player.OnDirectionChanged -= PlayMovementAnimation;
                _player.PlayerMovementManager.StopMovement();
                _player.CharacterAttackManager.OnBasicAttackPerform -= EnterBasicAttackPerformState;
                _player.CharacterAttackManager.OnChargeAttackCharge -= EnterChargeAttackState;
            }
        }

        public override void FrameUpdate()
        {
            if (!_player.IsOwner) return;

            _player.PlayerMovementManager.UpdateMovementDirectionViaInput();


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