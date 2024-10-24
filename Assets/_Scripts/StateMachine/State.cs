using System;
using Core.GameEventSystem;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public abstract class State
    {
        protected StateMachine _stateMachine;
        protected EventBus _eventBus;

        protected State(StateMachine stateMachine, EventBus eventBus)
        {
            _stateMachine = stateMachine;
            _eventBus = eventBus;
        }

        public virtual void EnterState()
        {
            _stateMachine.IsChangingState = false;
        }

        public virtual void ExitState() { }

        public virtual void FrameUpdate() { }

        public virtual void PhysicsUpdate() { }
    }
}