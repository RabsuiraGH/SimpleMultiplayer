using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerManager : CharacterManager
    {
        public PlayerMovementManager PlayerMovementManager { get; private set; }
        public PlayerInputManager InputManager { get; private set; }
        [field: SerializeField] public PlayerIdleState IdleState { get; private set; }
        [field: SerializeField] public PlayerMovementState MovementState { get; private set; }

        [field: SerializeField] public PlayerAnimatorManager PlayerAnimationManager { get; private set; }

        protected PlayerStatsManager PlayerStatsManager { get; private set; }


        protected override void Awake()
        {
            base.Awake();

            PlayerMovementManager = GetComponent<PlayerMovementManager>();
            PlayerAnimationManager = GetComponent<PlayerAnimatorManager>();
            PlayerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Start()
        {
            base.Start();
            IdleState = new PlayerIdleState(this, _characterStateMachine, null);
            MovementState = new PlayerMovementState(this, _characterStateMachine, null);
            _characterStateMachine.Initialize(IdleState);
        }

        protected override void Update()
        {
            base.Update();

            ReadAllInputs();
        }

        [Inject]
        public void Construct(PlayerInputManager inputManager)
        {
            InputManager = inputManager;
        }

        private void ReadAllInputs()
        {
            ReadMovementInput();
        }

        private void ReadMovementInput()
        {
            PlayerMovementManager.ReadMovementInput(InputManager.MovementInput);
        }
    }
}