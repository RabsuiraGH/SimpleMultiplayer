using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterStateMachine : StateMachine
    {
        [SerializeField] private CharacterManager _character;

        public enum CharacterStates
        {
            IdleState,
            MovementState,
            AttackState,
        }

        public override void Initialize(State startingState, NetworkBehaviour owner)
        {
            base.Initialize(startingState, owner);
            if (owner is CharacterManager character)
                _character = character;
        }

        public override State GetState(int state)
        {
            return state switch
            {
                (int)CharacterStates.IdleState => _character.IdleState,
                (int)CharacterStates.MovementState => _character.MovementState,
                (int)CharacterStates.AttackState => _character.AttackState,
                var _ => null
            };
        }

        public override void ChangeStateRPC(int state)
        {
            base.ChangeStateRPC(0);

            if (IsHost)
            {
                ChangeStateClientRPC(state);
            }
            else
            {
                ChangeStateServerRPC(state);
            }
        }

        [ClientRpc]
        public override void ChangeStateClientRPC(int state)
        {
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