using System;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterNetworkManager CharacterNetworkManager { get; private set; }

        public CharacterMovementManager CharacterMovementManager { get; private set; }

        public CharacterAnimatorManager CharacterAnimatorManager { get; private set; }

        [SerializeField] private CharacterStateMachine _characterStateMachine;

        public CharacterIdleState IdleState { get; private set; }

        public CharacterMovementState MovementState { get; private set; }

        [SerializeField] public Directions.MainDirection MainDirection { get; protected set; }

        [SerializeField] public Directions.SecondaryDirection SecDirection { get; protected set; }

        public event Action<Directions.MainDirection, Directions.SecondaryDirection> OnDirectionChanged;

        protected virtual void Awake()
        {
            CharacterNetworkManager = GetComponent<CharacterNetworkManager>();
            CharacterMovementManager = GetComponent<CharacterMovementManager>();
            CharacterAnimatorManager = GetComponent<CharacterAnimatorManager>();

            CharacterMovementManager.OnMovementDirectionChanged += ChangeFaceDirection;
        }

        protected virtual void Start()
        {
            IdleState = new(this, _characterStateMachine, null);
            MovementState = new(this, _characterStateMachine, null);
            _characterStateMachine.ChangeState(IdleState);
        }

        protected virtual void ChangeFaceDirection(Vector2 movementDirecction)
        {
            if (movementDirecction.x > 0)
                MainDirection = Directions.MainDirection.Right;
            else if (movementDirecction.x < 0)
                MainDirection = Directions.MainDirection.Left;

            if (movementDirecction.y > 0)
                SecDirection = Directions.SecondaryDirection.Up;
            else if (movementDirecction.y < 0)
                SecDirection = Directions.SecondaryDirection.Down;
            else if(movementDirecction.y == 0 && movementDirecction.x != 0)
                SecDirection = Directions.SecondaryDirection.Down;


            OnDirectionChanged?.Invoke(MainDirection, SecDirection);
        }

        protected virtual void Update()
        {

            if (IsOwner)
            {
                _characterStateMachine.CurrentState.FrameUpdate();
                    CharacterNetworkManager.NetworkPosition.Value = this.transform.position;
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

        public override void OnDestroy()
        {
            base.OnDestroy();
            CharacterMovementManager.OnMovementDirectionChanged -= ChangeFaceDirection;
        }
    }
}