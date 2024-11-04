using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class CharacterIdleState : CharacterState
    {
        private readonly CharacterIdleAnimation _idleAnimation = new();

        public CharacterIdleState(CharacterManager character, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(character, stateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            PlayIdleAnimation();

            if (_character.IsOwner)
            {
                _character.CharacterAttackManager.OnBasicAttackPerform += EnterBasicAttackPerformState;
                _character.CharacterAttackManager.OnChargeAttackCharge += EnterChargeAttackState;
            }
        }

        private void PlayIdleAnimation()
        {
            _idleAnimation.SetTags(_character.MainDirection.ToString(), _character.SecDirection.ToString());
            _character.CharacterAnimatorManager.PlayAnimation(_idleAnimation);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            Debug.Log((this, $"{_character.IsOwner}   {_character.CharacterMovementManager.IsMoving}"));
            if (!_character.IsOwner) return;

            if (_character.CharacterMovementManager.IsJumping && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC(_characterStateMachine.JumpState);
            }

            else if (_character.CharacterMovementManager.IsMoving && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC(_characterStateMachine.MovementState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!_character.IsOwner) return;

            _character.CharacterMovementManager.HandleAllMovement();
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

            if (_character.IsOwner)
            {
                _character.CharacterAttackManager.OnBasicAttackPerform -= EnterBasicAttackPerformState;
                _character.CharacterAttackManager.OnChargeAttackCharge -= EnterChargeAttackState;
            }
        }
    }
}