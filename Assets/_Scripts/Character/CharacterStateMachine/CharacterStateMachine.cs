using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterStateMachine : StateMachine
    {
        [SerializeField] private CharacterManager _character;

        public State IdleState { get; set; }
        public State MovementState { get; set; }
        public State JumpState { get; set; }
        public State AttackState { get; set; }
        public State ChargeAttackState { get; set; }
        public State DeathState { get; set; }

        private Dictionary<CharacterStates, State> _states;

        private enum CharacterStates
        {
            IdleState,
            MovementState,
            JumpState,
            AttackState,
            ChargeAttackState,
            DeathState
        }

        public override void Initialize(State startingState, NetworkBehaviour owner)
        {
            base.Initialize(startingState, owner);
            if (owner is CharacterManager character)
                _character = character;

            _states = new Dictionary<CharacterStates, State>
            {
                { CharacterStates.IdleState, IdleState },
                { CharacterStates.MovementState, MovementState },
                { CharacterStates.JumpState, JumpState },
                { CharacterStates.AttackState, AttackState },
                { CharacterStates.ChargeAttackState, ChargeAttackState },
                { CharacterStates.DeathState, DeathState }
            };
        }

        public override State GetState(int stateID)
        {
            return _states.TryGetValue((CharacterStates)stateID, out var state) ? state : null;
        }

        public override int GetStateID(State state)
        {
            return (int)_states.FirstOrDefault(x => x.Value == state).Key;
        }

        // TODO: ALLOW TO USE STATE CLASS INSTEAD OF INT ENUM
        public override void ChangeStateRPC(State state)
        {
            base.ChangeStateRPC(null);

            if (IsOwner)
            {
                CurrentState.ExitState();
                CurrentState = state;

                CurrentState.EnterState();

                if (IsHost)
                {
                    ChangeStateClientRPC(GetStateID(state));
                }
                else
                {
                    ChangeStateServerRPC(GetStateID(state));
                }
            }
        }

        [ClientRpc]
        public override void ChangeStateClientRPC(int state)
        {
            if (IsOwner) return;
            CurrentState.ExitState();
            CurrentState = GetState(state);

            CurrentState.EnterState();
        }

        [ServerRpc]
        public override void ChangeStateServerRPC(int state)
        {
            ChangeStateClientRPC(state);
        }
    }
}