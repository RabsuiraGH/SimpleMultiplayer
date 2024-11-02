using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public abstract class PlayerState : CharacterState
    {
        protected readonly PlayerManager _player;

        protected PlayerState(PlayerManager player, CharacterStateMachine playerStateMachine, EventBus eventBus)
            : base(player, playerStateMachine, eventBus)
        {
            _player = player;
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