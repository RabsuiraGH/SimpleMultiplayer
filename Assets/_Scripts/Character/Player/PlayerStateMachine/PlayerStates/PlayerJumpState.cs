using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerJumpState : PlayerState
    {
        private readonly CharacterJumpAnimation _jumpAnimation = new();

        public PlayerJumpState(PlayerManager player, CharacterStateMachine playerStateMachine, EventBus eventBus) :
            base(player, playerStateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            PlayJumpAnimation();
        }

        private void PlayJumpAnimation()
        {
            _jumpAnimation.SetTags(_player.MainDirection.ToString(), _player.SecDirection.ToString());
            _player.PlayerAnimationManager.PlayAnimation(_jumpAnimation);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (!_player.PlayerMovementManager.IsJumping && !_stateMachine.IsChangingState)
            {
                _characterStateMachine.ChangeStateRPC(_characterStateMachine.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}