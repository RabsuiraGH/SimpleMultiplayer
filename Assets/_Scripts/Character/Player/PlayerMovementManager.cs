using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerMovementManager : CharacterMovementManager
    {
        [SerializeField] private PlayerInputManager _inputManager;


        [SerializeField] private float _movementSpeed = 5;

        [Inject]
        public void Construct(PlayerInputManager inputManager)
        {
            _inputManager = inputManager;
        }


        protected override void Awake()
        {
            base.Awake();
        }

        protected override void ProceedMovement()
        {
            base.ProceedMovement();

            Vector2 newPosition = _rigidbody.position + _movementSpeed * Time.deltaTime * _inputManager.MovementInput ;
            _rigidbody.MovePosition(newPosition);
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