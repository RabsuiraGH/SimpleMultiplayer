using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class CharacterJumpState : CharacterState
    {
        private readonly CharacterJumpAnimation _jumpAnimation = new();

        public override void EnterState()
        {
            base.EnterState();
            PlayJumpAnimation();
        }

        private void PlayJumpAnimation()
        {
            _jumpAnimation.SetTags(_character.MainDirection.ToString(), _character.SecDirection.ToString());
            _character.CharacterAnimatorManager.PlayAnimation(_jumpAnimation);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (!_character.CharacterMovementManager.IsJumping && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC(_characterStateMachine.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public CharacterJumpState(CharacterManager character, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(character, stateMachine, eventBus)
        {
        }
    }
}