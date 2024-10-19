using UnityEngine;

namespace Core
{
    public class PlayerMovementManager : CharacterMovementManager
    {
        [SerializeField] private float _movementSpeed = 5;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void ProceedMovement()
        {
            base.ProceedMovement();

            if (_movementDirection.magnitude == 0) return;

            Vector2 newPosition = _rigidbody.position + _movementSpeed * Time.fixedDeltaTime * _movementDirection;
            _rigidbody.MovePosition(newPosition);
        }

        public void ReadMovementInput(Vector2 movementInput)
        {
            SetMovementDirection(movementInput);
        }

        protected override void HandleAllMovement()
        {
            base.HandleAllMovement();

            ProceedMovement();
        }

        protected override void FixedUpdate()
        {
            HandleAllMovement();
        }
    }
}