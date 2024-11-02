using System;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class StateMachine : NetworkBehaviour
    {
        [SerializeField] private NetworkBehaviour _owner;
        [field: SerializeField] public State CurrentState { get; protected set; }

        public bool IsChangingState { get; set; }

        public virtual void Initialize(State startingState, NetworkBehaviour owner)
        {
            _owner = owner;
            CurrentState = startingState;
            CurrentState.EnterState();
        }

        public virtual void ChangeStateRPC(State state)
        {
            IsChangingState = true;
        }

        [ClientRpc]
        public virtual void ChangeStateClientRPC(int state)
        {
            throw new NotImplementedException();
        }

        [ServerRpc]
        public virtual void ChangeStateServerRPC(int state)
        {
            throw new NotImplementedException();
        }

        public virtual State GetState(int stateID)
        {
            throw new NotImplementedException();
        }

        public virtual int GetStateID(State state)
        {
            throw new NotImplementedException();
        }
    }
}