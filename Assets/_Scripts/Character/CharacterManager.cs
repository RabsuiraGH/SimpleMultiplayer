using System;
using R3;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterManager : NetworkBehaviour
    {
        [SerializeField] protected CharacterStateMachine _characterStateMachine;

#if UNITY_EDITOR
        [SerializeField] private string _currentState;
#endif
        [field: SerializeField] public Directions.MainDirection MainDirection { get; protected set; }
        [field: SerializeField] public Directions.SecondaryDirection SecDirection { get; protected set; }
        public event Action<Directions.MainDirection, Directions.SecondaryDirection> OnDirectionChanged;

        public bool IsPerformingMainAction = false;

        public CharacterNetworkManager CharacterNetworkManager { get; private set; }

        public CharacterMovementManager CharacterMovementManager { get; private set; }

        public CharacterAnimatorManager CharacterAnimatorManager { get; private set; }

        public CharacterStatsManager CharacterStatsManager { get; private set; }

        public CharacterEffectsManager CharacterEffectsManager { get; private set; }
        public CharacterAttackManager CharacterAttackManager { get; private set; }
        public CharacterDeathManager CharacterDeathManager { get; private set; }

        protected virtual void Awake()
        {
            CharacterNetworkManager = GetComponent<CharacterNetworkManager>();
            CharacterMovementManager = GetComponent<CharacterMovementManager>();
            CharacterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            CharacterStatsManager = GetComponent<CharacterStatsManager>();
            CharacterEffectsManager = GetComponent<CharacterEffectsManager>();
            CharacterAttackManager = GetComponent<CharacterAttackManager>();
            CharacterDeathManager = GetComponent<CharacterDeathManager>();

            _characterStateMachine = GetComponent<CharacterStateMachine>();

        }

        protected virtual void Start()
        {
            InitializeStateMachine();


            if (IsOwner)
            {
                CharacterMovementManager.OnMovementDirectionChanged += ChangeFaceDirectionViaMovement;
            }

            // TODO: Maybe sub only if owner
            CharacterStatsManager.GetStats().MovementSpeed.CurrentValueReadonly
                                 .Subscribe(newValue => CharacterMovementManager.UpdateMovementSpeed(newValue));
            CharacterStatsManager.GetStats().JumpMovementSpeedMultiplier.CurrentValueReadonly
                                 .Subscribe(newValue => CharacterMovementManager.UpdateJumpMovementSpeedMultiplier(newValue));

            CharacterStatsManager.Health.OnValueChanged += CharacterDeathManager.CheckDeath;

            gameObject.name += CharacterNetworkManager.ObjectID.Value;
        }

        protected virtual void Update()
        {
            if (IsOwner)
            {
                _characterStateMachine.CurrentState.FrameUpdate();
                CharacterNetworkManager.NetworkPosition.Value = transform.position;
                CharacterNetworkManager.NetworkMainDirection.Value = MainDirection;
                CharacterNetworkManager.NetworkSecondaryDirection.Value = SecDirection;
            }
            // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALY BY THE POSITION OF ITS NETWORK TRANSFORM
            else
            {
                MainDirection = CharacterNetworkManager.NetworkMainDirection.Value;
                SecDirection = CharacterNetworkManager.NetworkSecondaryDirection.Value;
                transform.position = CharacterNetworkManager.NetworkPosition.Value;
            }

#if UNITY_EDITOR
            _currentState = _characterStateMachine.CurrentState.GetType().ToString();
#endif
        }

        protected void FixedUpdate()
        {
            if (IsOwner)
            {
                _characterStateMachine.CurrentState.PhysicsUpdate();
            }
        }

        public virtual void ChangeFaceDirectionViaMovement(Vector2 movementDirection)
        {
            if (movementDirection.x > 0)
            {
                MainDirection = Directions.MainDirection.Right;
            }
            else if (movementDirection.x < 0)
            {
                MainDirection = Directions.MainDirection.Left;
            }

            if (movementDirection.y > 0)
            {
                SecDirection = Directions.SecondaryDirection.Up;
            }
            else if (movementDirection.y < 0)
            {
                SecDirection = Directions.SecondaryDirection.Down;
            }
            else if (movementDirection.y == 0 && movementDirection.x != 0)

            {
                SecDirection = Directions.SecondaryDirection.Down;
            }

            OnDirectionChanged?.Invoke(MainDirection, SecDirection);
        }

        public void ExecuteEffect(InstantEffectSO effect)
        {
            effect.ProcessEffect(this);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (IsOwner)
            {
                CharacterMovementManager.OnMovementDirectionChanged -= ChangeFaceDirectionViaMovement;
            }
            CharacterStatsManager.Health.OnValueChanged -= CharacterDeathManager.CheckDeath;

        }

        protected virtual void InitializeStateMachine()
        {
            throw new NotImplementedException();
            // TODO: INIT BASE CHARACTER STATES
        }
    }
}