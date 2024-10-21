using R3;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerManager : CharacterManager
    {
        public PlayerMovementManager PlayerMovementManager { get; private set; }
        public PlayerInputManager InputManager { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMovementState MovementState { get; private set; }

        public PlayerAnimatorManager PlayerAnimationManager { get; private set; }

        protected PlayerStatsManager PlayerStatsManager { get; private set; }

        protected PlayerUIManager PlayerUIManager { get; private set; }

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

            PlayerUIManager.SetupUI(PlayerStatsManager);
        }

        protected override void Update()
        {
            base.Update();

            ReadAllInputs();
        }

        [Inject]
        public void Construct(PlayerInputManager inputManager, PlayerUIManager playerUIManager)
        {
            InputManager = inputManager;
            PlayerUIManager = playerUIManager;
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