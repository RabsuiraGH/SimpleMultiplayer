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

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}