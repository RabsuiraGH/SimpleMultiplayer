using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterMovementManager : NetworkBehaviour
    {
        [SerializeField] private CharacterManager _character;
        [SerializeField] protected Rigidbody2D _rigidbody;

        [SerializeField] protected Vector2 _movementDirection;

        [SerializeField] protected float _movementSpeed = 5;
        [SerializeField] private float _turnSpeed = 20f;
        [SerializeField] protected float _jumpMovementSpeedMultiplier = 5;

        public bool IsMoving => _movementDirection.magnitude > 0;
        public bool IsJumping = false;
        public event Action<Vector2> OnMovementDirectionChanged;
        public event Action<CharacterManager> OnJump;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _character = GetComponent<CharacterManager>();
        }

        protected void SetMovementDirection(Vector2 newDirection)
        {
            newDirection.Normalize();

            if (_movementDirection == newDirection)
            {
                return;
            }

            if (newDirection == Vector2.zero)
            {
                _movementDirection = newDirection;
            }
            else
            {
                _movementDirection = Vector2.Lerp(_movementDirection, newDirection, Time.deltaTime * _turnSpeed);
            }

            if (_movementDirection.magnitude <= 0.01f)
            {
                _movementDirection = Vector2.zero;
            }

            OnMovementDirectionChanged?.Invoke(newDirection);
        }

        public virtual void UpdateMovementSpeed(float speed)
        {
            _movementSpeed = speed;
        }

        public void UpdateJumpMovementSpeedMultiplier(float multiplier)
        {
            _jumpMovementSpeedMultiplier = multiplier;
        }

        public virtual void StopMovement()
        {
            _movementDirection = Vector2.zero;
        }

        public virtual void StopJumping()
        {
            IsJumping = false;
        }

        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
            HandleAllMovement();
        }

        public virtual void HandleAllMovement()
        {
            PerformMovement();
        }

        protected virtual void PerformMovement()
        {
            if (_movementDirection.magnitude == 0)
            {
                return;
            }

            Vector2 newPosition = _rigidbody.position + _movementSpeed * Time.fixedDeltaTime * _movementDirection;
            _rigidbody.MovePosition(newPosition);
        }

        public IEnumerator MovePositionOverTime(Vector2 startPosition, Vector2 endPosition, float time)
        {
            float elapsedTime = 0f;

            while (elapsedTime < time)
            {
                Vector2 newPosition = Vector2.Lerp(startPosition, endPosition, elapsedTime / time);
                _rigidbody.position = newPosition;

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _rigidbody.position = endPosition;
        }

        protected virtual void PerformJump()
        {
            _character.IsPerformingMainAction = true;
            IsJumping = true;
            OnJump?.Invoke(_character);
        }

        public virtual void PerformJumpRpc()
        {
            if (IsJumping || _character.IsPerformingMainAction) return;

            if (IsOwner)
            {
                PerformJump();
            }


            if (_character.IsHost)
            {
                PerformJumpClientRpc();
            }
            else
            {
                PerformJumpServerRpc();
            }
        }

        [ClientRpc]
        protected virtual void PerformJumpClientRpc()
        {
            if (IsOwner) return;
            PerformJump();
        }

        [ServerRpc]
        protected virtual void PerformJumpServerRpc()
        {
            PerformJumpClientRpc();
        }
    }
}