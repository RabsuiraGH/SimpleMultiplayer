using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterManager : NetworkBehaviour
    {
        [SerializeField] private CharacterNetworkManager _networkManager;

        protected virtual void Awake()
        {
            _networkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update()
        {
            if (IsOwner)
            {
                _networkManager.NetworkPosition.Value = this.transform.position;
            }
            // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALY BY THE POSITION OF ITS NETWORK TRANSFORM
            else
            {
                // POSITION
                transform.position = Vector3.SmoothDamp
                    (transform.position,
                    _networkManager.NetworkPosition.Value,
                    ref _networkManager.NetworkPositionVelocity,
                    _networkManager.NetworkPositionSmoothTime);
            }
        }
    }
}