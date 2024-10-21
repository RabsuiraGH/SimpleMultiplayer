using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerIdleState : CharacterState
    {
        [SerializeField] private readonly CharacterIdleAnimation _idleAnimation = new();
        [SerializeField] private readonly PlayerManager _player;

        public PlayerIdleState(PlayerManager player, StateMachine stat, EventBus eventBus) :
            base(player, stat, eventBus)
        {
            _player = player;
        }

        public override void EnterState()
        {
            _idleAnimation.SetTags(_character.MainDirection.ToString(), _character.SecDirection.ToString());
            _player.PlayerAnimationManager.PlayAnimation(_idleAnimation);
        }

        public override void ExitState() { }

        public override void FrameUpdate()
        {
            _player.PlayerMovementManager.HandleAllMovement();

            if (_player.PlayerMovementManager.IsMoving)
            {
                _stateMachine.ChangeState(_player.MovementState);
            }
        }

        public override void PhysicsUpdate()
        {
            _player.PlayerMovementManager.HandleAllMovement();
        }
    }
}