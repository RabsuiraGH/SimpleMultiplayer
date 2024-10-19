using System;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterMovementManager : NetworkBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody;

        [SerializeField] protected Vector2 _movementDirection;

        public event Action<Vector2> OnMovementDirectionChanged;

        public bool IsMoving { get => _movementDirection.magnitude > 0; }

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected void SetMovementDirection(Vector2 newDirection)
        {
            if (_movementDirection.Equals(newDirection)) return;


            if (newDirection.Equals(Vector2.zero))
            {
                _movementDirection = newDirection;
            }
            else
            {
                _movementDirection = Vector2.Lerp(_movementDirection, newDirection, Time.deltaTime * 20);
            }

            if (_movementDirection.magnitude < 0.01f)
            {
                _movementDirection = Vector2.zero;
            }

            OnMovementDirectionChanged.Invoke(newDirection);
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