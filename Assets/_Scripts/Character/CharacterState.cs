using System;
using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class CharacterState
    {
        [SerializeField] protected CharacterManager _character = null;
        [SerializeField] protected CharacterStateMachine _playerStateMachine = null;
        [SerializeField] protected EventBus _eventBus = null;

        public CharacterState(CharacterManager character, CharacterStateMachine characterStateMachine, EventBus eventBus)
        {
            _character = character;
            _playerStateMachine = characterStateMachine;
            _eventBus = eventBus;
        }

        public virtual void EnterState()
        { }

        public virtual void ExitState()
        { }

        public virtual void FrameUpdate()
        { }

        public virtual void PhysicsUpdate()
        { }

    }
}
