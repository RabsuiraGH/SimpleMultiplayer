using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerAttackState : PlayerState
    {
        private readonly CharacterAttackAnimation _attackAnimation = new();

        public PlayerAttackState(PlayerManager player, CharacterStateMachine stat, EventBus eventBus) :
            base(player, stat, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            PlayAttackAnimation();
        }

        private void PlayAttackAnimation()
        {
            _attackAnimation.SetTags(_player.MainDirection.ToString(), _player.SecDirection.ToString());
            _player.PlayerAnimationManager.PlayAnimation(_attackAnimation);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            if (!_player.IsOwner) return;

            if (!_player.CharacterAttackManager.IsAttacking && !_stateMachine.IsChangingState)
            {
                _stateMachine.ChangeStateRPC((int)CharacterStateMachine.CharacterStates.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            if (!_player.IsOwner) return;
        }
    }
}