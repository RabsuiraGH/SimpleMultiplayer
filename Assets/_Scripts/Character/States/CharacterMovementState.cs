using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class CharacterMovementState : CharacterState
    {
        [SerializeField] private CharacterWalkAnimation _movingAnimation = new();

        public CharacterMovementState(CharacterManager character, CharacterStateMachine characterStateMachine, EventBus eventBus) : base(character, characterStateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            _character.OnDirectionChanged += PlayAnimation;
            PlayAnimation(_character.MainDirection, _character.SecDirection);
        }

        private void PlayAnimation(Directions.MainDirection direction1, Directions.SecondaryDirection direction2)
        {
            _movingAnimation.SetTags(_character.MainDirection.ToString(), _character.SecDirection.ToString());

            _character.CharacterAnimatorManager.PlayAnimation(_movingAnimation);
        }

        public override void ExitState()
        {
        }

        public override void FrameUpdate()
        {
            if (!_character.CharacterMovementManager.IsMoving)
            {
                _playerStateMachine.ChangeState(_character.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
        }
    }
}