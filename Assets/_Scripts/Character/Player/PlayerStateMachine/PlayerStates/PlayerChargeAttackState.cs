using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerChargeAttackState : PlayerState
    {
        private readonly CharacterChargeAttackAnimation _chargeAttackAnimation = new();

        public PlayerChargeAttackState(PlayerManager player, StateMachine playerStateMachine, EventBus eventBus) : base(
            player, playerStateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            PlayChargeAnimation();

            _player.CharacterAttackManager.OnChargeAttackPerform += PerformAttackAnimation;
        }

        private void PlayChargeAnimation()
        {
            _chargeAttackAnimation.SetTags(_chargeAttackAnimation.ChargeTag, _player.MainDirection.ToString(),
                                           _player.SecDirection.ToString());
            _player.PlayerAnimationManager.PlayAnimation(_chargeAttackAnimation);
        }

        private void PerformAttackAnimation()
        {
            _chargeAttackAnimation.SetTags(_chargeAttackAnimation.PerformedTag, _player.MainDirection.ToString(),
                                           _player.SecDirection.ToString());
            _player.PlayerAnimationManager.PlayAnimation(_chargeAttackAnimation);
        }

        public override void ExitState()
        {
            base.ExitState();
            _player.CharacterAttackManager.OnChargeAttackPerform -= PerformAttackAnimation;
        }

        public override void FrameUpdate()
        {
            if (!_player.CharacterAttackManager.IsAttacking && !_stateMachine.IsChangingState)
            {
                if (!_player.CharacterAttackManager.IsCharging)
                {
                    _stateMachine.ChangeStateRPC((int)CharacterStateMachine.CharacterStates.IdleState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
        }
    }
}