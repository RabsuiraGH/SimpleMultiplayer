using UnityEngine;

namespace Core
{
    public class PlayerMovementManager : CharacterMovementManager
    {

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
        }

        protected override void ProceedMovement()
        {
            base.ProceedMovement();

            if (_movementDirection.magnitude == 0)
            {
                return;
            }

            Vector2 newPosition = _rigidbody.position + _movementSpeed * Time.fixedDeltaTime * _movementDirection;
            _rigidbody.MovePosition(newPosition);
        }

        public void ReadMovementInput(Vector2 movementInput)
        {
            SetMovementDirection(movementInput);
        }

        public override void HandleAllMovement()
        {
            base.HandleAllMovement();

            ProceedMovement();
        }
    }
}