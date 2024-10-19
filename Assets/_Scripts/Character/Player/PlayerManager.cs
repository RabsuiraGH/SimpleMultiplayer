using Zenject;

namespace Core
{
    public class PlayerManager : CharacterManager
    {
        public PlayerMovementManager PlayerMovementManager { get; private set; }

        public PlayerInputManager InputManager { get; private set; }

        [Inject]
        public void Construct(PlayerInputManager inputManager)
        {
            InputManager = inputManager;
        }

        protected override void Awake()
        {
            base.Awake();

            PlayerMovementManager = GetComponent<PlayerMovementManager>();
        }

        protected override void Update()
        {
            base.Update();

            ReadAllInputs();
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