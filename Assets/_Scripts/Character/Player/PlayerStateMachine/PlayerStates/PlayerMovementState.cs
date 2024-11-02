using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerMovementState : PlayerState
    {
        private readonly CharacterWalkAnimation _movingAnimation = new();

        public PlayerMovementState(PlayerManager player, CharacterStateMachine stateMachine, EventBus eventBus)
            : base(player, stateMachine, eventBus)
        {
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
            _characterStateMachine.ChangeStateRPC(_characterStateMachine.ChargeAttackState);
        }

        private void EnterBasicAttackPerformState()
        {
            _characterStateMachine.ChangeStateRPC(_characterStateMachine.AttackState);
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
            base.FrameUpdate();
            if (!_player.IsOwner) return;

            _player.PlayerMovementManager.UpdateMovementDirectionViaInput();

            if (_player.PlayerMovementManager.IsJumping && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC(_characterStateMachine.JumpState);
            }

            else if (!_player.PlayerMovementManager.IsMoving && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC(_characterStateMachine.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!_player.IsOwner) return;

            _player.PlayerMovementManager.HandleAllMovement();
        }
    }
}