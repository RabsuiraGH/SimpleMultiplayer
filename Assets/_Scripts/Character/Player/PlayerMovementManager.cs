using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class PlayerMovementManager : CharacterMovementManager
    {
        private PlayerManager _player;
        private Vector2 _movementInput;
        public bool HasPlayerInput => _movementInput.magnitude > 0;

        protected override void Awake()
        {
            base.Awake();
            _player = GetComponent<PlayerManager>();
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

        protected override void PerformJump()
        {
            base.PerformJump();
            if (IsOwner)
            {
#pragma warning disable CS4014
                MoveWhileJump(_movementDirection);
#pragma warning restore CS4014
            }
        }

        public override void PerformJumpRpc()
        {
            if (IsJumping || _player.IsPerformingMainAction) return;
            if (!IsOwner) return;

            PerformJump();


            if (_player.IsHost)
            {
                PerformJumpClientRpc();
            }
            else
            {
                PerformJumpServerRpc();
            }
        }

        [ClientRpc]
        protected override void PerformJumpClientRpc()
        {
            if (IsOwner) return;
            PerformJump();
        }

        [ServerRpc]
        protected override void PerformJumpServerRpc()
        {
            PerformJumpClientRpc();
        }

        private async Task MoveWhileJump(Vector2 direction)
        {
            while (IsJumping)
            {
                Vector2 newPosition = _rigidbody.position + (_movementSpeed * _jumpMovementSpeedMultiplier * Time.fixedDeltaTime * direction);
                _rigidbody.MovePosition(newPosition);
                await Task.Yield();
            }
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