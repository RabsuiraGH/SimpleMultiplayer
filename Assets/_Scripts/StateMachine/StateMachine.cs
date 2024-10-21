using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class StateMachine
    {
        [field: SerializeField] public State CurrentState { get; set; }

        public void Initialize(State startingState)
        {
            CurrentState = startingState;
            CurrentState.EnterState();
        }

        public void ChangeState(State newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}