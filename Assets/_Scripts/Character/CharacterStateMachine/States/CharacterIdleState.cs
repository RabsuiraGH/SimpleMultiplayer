using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class CharacterIdleState : CharacterState
    {
        [SerializeField] private CharacterIdleAnimation _idleAnimation = new();

        public CharacterIdleState(CharacterManager character, CharacterStateMachine characterStateMachine, EventBus eventBus) : base(character, characterStateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            _idleAnimation.SetTags(_character.MainDirection.ToString(), _character.SecDirection.ToString());
            _character.CharacterAnimatorManager.PlayAnimation(_idleAnimation);
        }

        public override void ExitState()
        {
        }

        public override void FrameUpdate()
        {
            if (_character.CharacterMovementManager.IsMoving)
            {
                _playerStateMachine.ChangeState(_character.MovementState);
            }
        }

        public override void PhysicsUpdate()
        {
        }
    }
}