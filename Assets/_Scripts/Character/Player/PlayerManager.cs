using R3;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerManager : CharacterManager
    {
        public PlayerInputManager InputManager { get; private set; }
        public PlayerMovementManager PlayerMovementManager { get; private set; }

        public PlayerAnimatorManager PlayerAnimationManager { get; private set; }

        public PlayerStatsManager PlayerStatsManager { get; private set; }

        public PlayerUIManager PlayerUIManager { get; private set; }
        public PlayerTeamUIManager PlayerTeamUIManager { get; private set; }
        public PlayerAttackManager PlayerAttackManager { get; private set; }

        [Inject]
        public void Construct(PlayerInputManager inputManager, PlayerUIManager playerUIManager)
        {
            InputManager = inputManager;
            PlayerUIManager = playerUIManager;
        }

        protected override void Awake()
        {
            base.Awake();

            PlayerMovementManager = GetComponent<PlayerMovementManager>();
            PlayerAnimationManager = GetComponent<PlayerAnimatorManager>();
            PlayerStatsManager = GetComponent<PlayerStatsManager>();
            PlayerAttackManager = GetComponent<PlayerAttackManager>();

            PlayerTeamUIManager = GetComponentInChildren<PlayerTeamUIManager>();

            _characterStateMachine = GetComponent<CharacterStateMachine>();
        }

        protected override void Start()
        {
            base.Start();

            // Setups State Machine


            if (IsOwner)
            {
                PlayerUIManager.SetupUI(PlayerStatsManager);

                SubscribeInput();
            }
            else
            {
                PlayerTeamUIManager.SetupTeamUI(PlayerStatsManager);
            }
        }

        protected override void InitializeStateMachine()
        {
            _characterStateMachine.IdleState = new PlayerIdleState(this, _characterStateMachine, null);
            _characterStateMachine.MovementState = new PlayerMovementState(this, _characterStateMachine, null);
            _characterStateMachine.JumpState = new PlayerJumpState(this, _characterStateMachine, null);
            _characterStateMachine.AttackState = new PlayerAttackState(this, _characterStateMachine, null);
            _characterStateMachine.ChargeAttackState = new PlayerChargeAttackState(this, _characterStateMachine, null);
            _characterStateMachine.DeathState = new PlayerDeathState(this, _characterStateMachine, null);

            _characterStateMachine.Initialize(_characterStateMachine.IdleState, this);
        }

        protected override void Update()
        {
            base.Update();

            if (IsOwner)
            {
                HandleAllInputs();
            }
        }

        private void SubscribeInput()
        {
            // JUMP BEHAVIOURS
            InputManager.OnJumpButtonPressed += PlayerMovementManager.PerformJumpRpc;

            // BASIC ATTACK BEHAVIOURS
            InputManager.OnAttackButtonPressed += CharacterAttackManager.PerformAttackRpc;

            // CHARGE ATTACK INPUT BEHAVIOURS
            InputManager.OnChargeAttackChargePressed += CharacterAttackManager.StartChargeAttackCharge;
            InputManager.OnChargeAttackPerformPressed += CharacterAttackManager.TryPerformChargeAttack;
            InputManager.OnChargeAttackChargeReleased += CharacterAttackManager.CancelChargeAttack;
        }

        private void HandleAllInputs()
        {
            HandleMovementInput();
        }

        private void HandleMovementInput()
        {
            PlayerMovementManager.ApplyMovementInput(InputManager.MovementInput);
        }

        private void UnsubscribeInput()
        {
            InputManager.OnAttackButtonPressed -= CharacterAttackManager.PerformAttackRpc;
            InputManager.OnChargeAttackChargePressed -= CharacterAttackManager.StartChargeAttackCharge;
            InputManager.OnChargeAttackPerformPressed -= CharacterAttackManager.TryPerformChargeAttack;
            InputManager.OnChargeAttackChargeReleased -= CharacterAttackManager.CancelChargeAttack;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            UnsubscribeInput();
        }
    }
}