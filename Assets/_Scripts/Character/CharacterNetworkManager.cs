using System;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        public NetworkVariable<Vector3> NetworkPosition { get; private set; } =
            new(Vector3.zero,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        public NetworkVariable<Directions.MainDirection> NetworkMainDirection { get; private set; } =
            new(Directions.MainDirection.Right,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        public NetworkVariable<Directions.SecondaryDirection> NetworkSecondaryDirection { get; private set; } =
            new(Directions.SecondaryDirection.Down,
                NetworkVariableReadPermission.Everyone,
                NetworkVariableWritePermission.Owner);

        public NetworkVariable<int> ObjectID { get; private set; }

        protected void Awake()
        {
            ObjectID = new NetworkVariable<int>(gameObject.GetInstanceID());
        }
    }
}