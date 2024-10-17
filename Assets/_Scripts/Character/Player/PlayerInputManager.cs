using Core.InputSystem;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private BaseControls _baseControls;

        [field: SerializeField] public Vector2 MovementInput { get; private set; }

        [Inject]
        public void Construct(BaseControls baseControls)
        {
            _baseControls = baseControls;
        }

        private void Awake()
        {
            ReadPlayerInput();
        }

        private void ReadPlayerInput()
        {
            // READ PLAYER MOVEMENT
            _baseControls.Gameplay.Movement.performed += i => MovementInput = i.ReadValue<Vector2>();

            // READ PLAYER ATTACK INPUT
            // READ PLAYER CHARGE ATTACK INPUT
            // READ PLAYER JUMP INPUT
            // READ PLAYER USE ITEM INPUT
            // ETC
        }
    }
}