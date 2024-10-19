using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Core
{
    [Serializable]
    public class CharacterStateMachine
    {
        [field: SerializeField] public CharacterState CurrentState { get; set; } = null;

        public void Initialize(CharacterState startingState)
        {
            CurrentState = startingState;
            CurrentState.EnterState();
        }

        public void ChangeState(CharacterState newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}
