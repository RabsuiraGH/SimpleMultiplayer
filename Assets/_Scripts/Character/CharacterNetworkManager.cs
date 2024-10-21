using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        [Header("POSITION")]
        [field: SerializeField]
        public NetworkVariable<Vector3> NetworkPosition { get; private set; } =
            new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [field: SerializeField] public Vector3 NetworkPositionVelocity;


        // TIME TO SYNC POSITIONS
        [field: SerializeField] public float NetworkPositionSmoothTime { get; private set; } = 0.05f;

        // TODO: IMPLEMENT NETWORK MOVEMENT VARIABLES 
    }
}