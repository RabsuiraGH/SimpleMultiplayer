using System;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public abstract class CharacterManager : NetworkBehaviour
    {
        [SerializeField] protected StateMachine _characterStateMachine;
        [field: SerializeField] public Directions.MainDirection MainDirection { get; protected set; }
        [field: SerializeField] public Directions.SecondaryDirection SecDirection { get; protected set; }
        public CharacterNetworkManager CharacterNetworkManager { get; private set; }

        protected CharacterMovementManager CharacterMovementManager { get; private set; }

        protected CharacterAnimatorManager CharacterAnimatorManager { get; private set; }

        protected virtual void Awake()
        {
            CharacterNetworkManager = GetComponent<CharacterNetworkManager>();
            CharacterMovementManager = GetComponent<CharacterMovementManager>();
            CharacterAnimatorManager = GetComponent<CharacterAnimatorManager>();

            CharacterMovementManager.OnMovementDirectionChanged += ChangeFaceDirection;
        }

        protected virtual void Start() { }

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
    }
}