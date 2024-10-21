using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public abstract class PlayerState : State
    {
        [SerializeField] protected PlayerManager _player;

        public PlayerState(PlayerManager player, StateMachine playerStateMachine, EventBus eventBus)
            : base(playerStateMachine, eventBus)
        {
            _player = player;
        }

        public override void EnterState() { }

        public override void ExitState() { }

        public override void FrameUpdate() { }

        public override void PhysicsUpdate() { }
    }
}