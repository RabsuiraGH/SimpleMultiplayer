using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterMovementManager : NetworkBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody;

        [SerializeField] protected Vector2 _movementDirection;

        [SerializeField] protected float _movementSpeed = 5;

        public bool IsMoving => _movementDirection.magnitude > 0;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
        }

        protected virtual void FixedUpdate()
        {
        }

        public event Action<Vector2> OnMovementDirectionChanged;

        protected void SetMovementDirection(Vector2 newDirection)
        {
            if (_movementDirection.Equals(newDirection))
            {
                return;
            }

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

            OnMovementDirectionChanged?.Invoke(newDirection);
        }

        public void UpdateMovementSpeed(float speed)
        {
            _movementSpeed = speed;
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

        public IEnumerator MovePositionOverTime(Vector2 startPosition, Vector2 endPosition, float time, float pushPoint, float pushAmount)
        {
            float elapsedTime = 0f;
            float pushTime = time * pushPoint;
            float afterPushTime = time - pushTime;

            Vector2 pushPosition = Vector2.Lerp(startPosition, endPosition, pushAmount);

            while (elapsedTime < time)
            {
                Vector2 newPosition;

                if (elapsedTime < pushTime)
                {

                    newPosition = Vector2.Lerp(startPosition, pushPosition, elapsedTime / pushTime);
                }
                else
                {
                    float timeAfterPush = elapsedTime - pushTime;
                    newPosition = Vector2.Lerp(pushPosition, endPosition, timeAfterPush / afterPushTime);
                }

                transform.position = newPosition;
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            transform.position = endPosition;
        }

        public virtual void HandleAllMovement()
        {
        }

        protected virtual void ProceedMovement()
        {
        }
    }
}