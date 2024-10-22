using System;
using Codice.CM.Client.Differences;
using R3;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public abstract class CharacterManager : NetworkBehaviour
    {
        [SerializeField] protected StateMachine _characterStateMachine;
        [field: SerializeField] public Directions.MainDirection MainDirection { get; protected set; }
        [field: SerializeField] public Directions.SecondaryDirection SecDirection { get; protected set; }
        [field: SerializeField] public CharacterNetworkManager CharacterNetworkManager { get; private set; }

        [field: SerializeField] protected CharacterMovementManager CharacterMovementManager { get; private set; }

        [field: SerializeField] protected CharacterAnimatorManager CharacterAnimatorManager { get; private set; }

        [field: SerializeField] public CharacterStatsManager CharacterStatsManager { get; private set; }

        [field: SerializeField]public CharacterEffectsManager CharacterEffectsManager { get; private set; }

        protected virtual void Awake()
        {
            CharacterNetworkManager = GetComponent<CharacterNetworkManager>();
            CharacterMovementManager = GetComponent<CharacterMovementManager>();
            CharacterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            CharacterStatsManager = GetComponent<CharacterStatsManager>();
            CharacterEffectsManager = GetComponent<CharacterEffectsManager>();
        }

        protected virtual void Start()
        {
            CharacterMovementManager.OnMovementDirectionChanged += ChangeFaceDirection;

            CharacterStatsManager.GetStats().MovementSpeed.CurrentValueReadonly
                                 .Subscribe(newValue => CharacterMovementManager.UpdateMovementSpeed(newValue));
        }

        protected virtual void Update()
        {
            if (IsOwner)
            {
                _characterStateMachine.CurrentState.FrameUpdate();
                CharacterNetworkManager.NetworkPosition.Value = transform.position;
            }
            // IF THIS CHARACTER IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALY BY THE POSITION OF ITS NETWORK TRANSFORM
            else
            {
                // POSITION
                /*                transform.position = Vector3.SmoothDamp
                                    (transform.position,
                                    CharacterNetworkManager.NetworkPosition.Value,
                                    ref CharacterNetworkManager.NetworkPositionVelocity,
                                    CharacterNetworkManager.NetworkPositionSmoothTime);*/

                transform.position = CharacterNetworkManager.NetworkPosition.Value;
            }
        }

        protected void FixedUpdate()
        {
            if (IsOwner)
            {
                _characterStateMachine.CurrentState.PhysicsUpdate();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            CharacterMovementManager.OnMovementDirectionChanged -= ChangeFaceDirection;
        }

        public event Action<Directions.MainDirection, Directions.SecondaryDirection> OnDirectionChanged;

        protected virtual void ChangeFaceDirection(Vector2 movementDirection)
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
    }
}