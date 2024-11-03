using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public abstract class CharacterState : State
    {
        protected readonly CharacterManager _character;
        protected readonly CharacterStateMachine _characterStateMachine;

        protected CharacterState(CharacterManager character, CharacterStateMachine stateMachine,
                                 EventBus eventBus) : base(
            stateMachine, eventBus)
        {
            _character = character;
            _characterStateMachine = stateMachine;
        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            if (!_character.IsOwner) return;
            CheckDeathState();
        }

        private void CheckDeathState()
        {
            if (_stateMachine.CurrentState == _characterStateMachine.DeathState) return;

            if (_character.CharacterDeathManager.IsDead && !_stateMachine.IsChangingState)
            {
                _stateMachine.ChangeStateRPC(_characterStateMachine.DeathState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}