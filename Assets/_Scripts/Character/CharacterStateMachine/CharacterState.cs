using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public abstract class CharacterState : State
    {
        [SerializeField] protected readonly CharacterManager _character;

        protected CharacterState(CharacterManager player, StateMachine stat, EventBus eventBus) : base(
            stat, eventBus)
        {
            _character = player;
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