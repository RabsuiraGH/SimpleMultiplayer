using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerIdleState : PlayerState
    {
        private readonly CharacterIdleAnimation _idleAnimation = new();

        public PlayerIdleState(PlayerManager player, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(player, stateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            PlayIdleAnimation();

            if (_player.IsOwner)
            {
                _player.CharacterAttackManager.OnBasicAttackPerform += EnterBasicAttackPerformState;
                _player.CharacterAttackManager.OnChargeAttackCharge += EnterChargeAttackState;
            }
        }

        private void PlayIdleAnimation()
        {
            _idleAnimation.SetTags(_player.MainDirection.ToString(), _player.SecDirection.ToString());
            _player.PlayerAnimationManager.PlayAnimation(_idleAnimation);
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

            else if (_player.PlayerMovementManager.IsMoving && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC(_characterStateMachine.MovementState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!_player.IsOwner) return;

            _player.PlayerMovementManager.HandleAllMovement();
        }

        private void EnterBasicAttackPerformState()
        {
            _characterStateMachine.ChangeStateRPC(_characterStateMachine.AttackState);
        }

        private void EnterChargeAttackState()
        {
            _characterStateMachine.ChangeStateRPC(_characterStateMachine.ChargeAttackState);
        }

        public override void ExitState()
        {
            base.ExitState();

            if (_player.IsOwner)
            {
                _player.CharacterAttackManager.OnBasicAttackPerform -= EnterBasicAttackPerformState;
                _player.CharacterAttackManager.OnChargeAttackCharge -= EnterChargeAttackState;
            }
        }
    }
}