using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerMovementState : CharacterState
    {
        [SerializeField] private readonly CharacterWalkAnimation _movingAnimation = new();
        [SerializeField] private readonly PlayerManager _player;

        public PlayerMovementState(PlayerManager player, StateMachine characterStateMachine, EventBus eventBus)
            : base(player, characterStateMachine, eventBus)
        {
            _player = player;
        }

        public override void EnterState()
        {
            _character.OnDirectionChanged += PlayAnimation;
            PlayAnimation(_character.MainDirection, _character.SecDirection);
        }

        private void PlayAnimation(Directions.MainDirection direction1, Directions.SecondaryDirection direction2)
        {
            _movingAnimation.SetTags(_character.MainDirection.ToString(), _character.SecDirection.ToString());

            _player.PlayerAnimationManager.PlayAnimation(_movingAnimation);
        }

        public override void ExitState() { }

        public override void FrameUpdate()
        {
            if (!_player.PlayerMovementManager.IsMoving)
            {
                _stateMachine.ChangeState(_player.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            _player.PlayerMovementManager.HandleAllMovement();
        }
    }
}