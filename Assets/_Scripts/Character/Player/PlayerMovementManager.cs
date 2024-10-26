using UnityEngine;

namespace Core
{
    public class PlayerMovementManager : CharacterMovementManager
    {
        private Vector2 _movementInput;
        public bool HasPlayerInput => _movementInput.magnitude > 0;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void PerformMovement()
        {
            if (_movementDirection.magnitude == 0)
            {
                return;
            }

            Vector2 newPosition = _rigidbody.position + _movementSpeed * Time.fixedDeltaTime * _movementDirection;
            _rigidbody.MovePosition(newPosition);
        }

        public void ApplyMovementInput(Vector2 movementInput)
        {
            _movementInput = movementInput;
        }

        public void UpdateMovementDirectionViaInput()
        {
            SetMovementDirection(_movementInput);
        }

        public override void HandleAllMovement()
        {
            base.HandleAllMovement();

            PerformMovement();
        }

        protected override void Update()
        {
        }

        protected override void FixedUpdate()
        {
        }
    }
}