using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterMovementManager : NetworkBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void HandleAllMovement()
        {
        }

        protected virtual void ProceedMovement()
        {
        }


        protected virtual void Update()
        {

        }


        protected virtual void FixedUpdate()
        {
        }
    }
}