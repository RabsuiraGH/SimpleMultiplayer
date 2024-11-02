using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerDeathState : PlayerState
    {
        private readonly PlayerDeathAnimation _deathAnimation = new();

        public PlayerDeathState(PlayerManager player, CharacterStateMachine playerStateMachine, EventBus eventBus) :
            base(
                player, playerStateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            PlayDeathAnimation();
        }

        private void PlayDeathAnimation()
        {
            if (Random.Range(0, 2) == 0)
            {
                _deathAnimation.SetTags(PlayerDeathAnimation.DEATH_TYPE);
            }
            else
            {
                _deathAnimation.SetTags(PlayerDeathAnimation.DEATH_TYPE_SOUL);
            }


            _player.PlayerAnimationManager.PlayAnimation(_deathAnimation);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            if (!_player.IsOwner) return;
            if (!_player.CharacterDeathManager.IsDead && !_stateMachine.IsChangingState)
            {
                _stateMachine.ChangeStateRPC(_characterStateMachine.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}