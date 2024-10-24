using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public abstract class CharacterState : State
    {
        protected readonly CharacterManager _character;

        protected CharacterState(CharacterManager character, StateMachine stateMachine, EventBus eventBus) : base(
            stateMachine, eventBus)
        {
            _character = character;
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