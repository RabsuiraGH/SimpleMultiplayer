using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class CharacterMovementState : CharacterState
    {
        private readonly CharacterWalkAnimation _movingAnimation = new();

        public CharacterMovementState(CharacterManager character, CharacterStateMachine stateMachine, EventBus eventBus) : base(character, stateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            if (_character.IsOwner)
            {
                _character.OnDirectionChanged += PlayMovementAnimation;
                _character.CharacterAttackManager.OnBasicAttackPerform += EnterBasicAttackPerformState;
                _character.CharacterAttackManager.OnChargeAttackCharge += EnterChargeAttackState;
            }
        }

        private void PlayMovementAnimation(Directions.MainDirection mainDirection,
                                           Directions.SecondaryDirection secondaryDirection)
        {
            _movingAnimation.SetTags(mainDirection.ToString(), secondaryDirection.ToString());

            _character.CharacterAnimatorManager.PlayAnimation(_movingAnimation);
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


            if (_character.IsOwner)
            {
                _character.OnDirectionChanged -= PlayMovementAnimation;
                _character.CharacterMovementManager.StopMovement();
                _character.CharacterAttackManager.OnBasicAttackPerform -= EnterBasicAttackPerformState;
                _character.CharacterAttackManager.OnChargeAttackCharge -= EnterChargeAttackState;
            }
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            if (!_character.IsOwner) return;

            if (_character.CharacterMovementManager.IsJumping && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC(_characterStateMachine.JumpState);
            }

            else if (!_character.CharacterMovementManager.IsMoving && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC(_characterStateMachine.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!_character.IsOwner) return;

            _character.CharacterMovementManager.HandleAllMovement();
        }
    }
}