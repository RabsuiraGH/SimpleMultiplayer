using System;
using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    [Serializable]
    public abstract class State
    {
        [SerializeField] protected StateMachine _stateMachine;
        [SerializeField] protected EventBus _eventBus;

        public State(StateMachine stateMachine, EventBus eventBus)
        {
            _stateMachine = stateMachine;
            _eventBus = eventBus;
        }

        public virtual void EnterState() { }

        public virtual void ExitState() { }

        public virtual void FrameUpdate() { }

        public virtual void PhysicsUpdate() { }
    }
}