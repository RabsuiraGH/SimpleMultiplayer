using System;
using Core.InputSystem;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerInputManager : MonoBehaviour
    {
        [field: SerializeField] public Vector2 MovementInput { get; private set; }
        public event Action OnAttackButtonPressed;
        public event Action OnChargeAttackChargePressed;
        public event Action OnChargeAttackReleasePressed;
        public event Action OnChargeAttackChargeReleased;

        [SerializeField] private BaseControls _baseControls;

        private void Awake()
        {
            ReadPlayerInput();
        }

        [Inject]
        public void Construct(BaseControls baseControls)
        {
            _baseControls = baseControls;
        }

        private void ReadPlayerInput()
        {
            // READ PLAYER MOVEMENT
            _baseControls.Gameplay.Movement.performed += i => MovementInput = i.ReadValue<Vector2>();

            // READ PLAYER ATTACK INPUT
            _baseControls.Gameplay.Attack.performed += i => OnAttackButtonPressed?.Invoke();

            // READ START CHARGE ATTACK INPUT
            _baseControls.Gameplay.ChargeAttack.started += i =>OnChargeAttackChargePressed?.Invoke();

            // READ INPUT TO PERFORM CHARGE ATTACK IF IT WILL BE CHARGED
            _baseControls.Gameplay.Attack.performed += i => OnChargeAttackReleasePressed?.Invoke();

            // CANCEL CHARGE OF CHARGED ATTACK INPUT
            _baseControls.Gameplay.ChargeAttack.canceled += i => OnChargeAttackChargeReleased?.Invoke();

            // READ PLAYER JUMP INPUT
            // READ PLAYER USE ITEM INPUT
            // ETC
        }
    }
}