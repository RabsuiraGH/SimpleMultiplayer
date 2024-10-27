using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerChargeAttackState : PlayerState
    {
        private readonly CharacterChargeAttackAnimation _chargeAttackAnimation = new();
        private readonly CharacterStateMachine _characterStateMachine;

        private static float _chargeAnimationDuration = -1f;
        private float _chargeAnimationSpeed;

        public PlayerChargeAttackState(PlayerManager player, CharacterStateMachine playerStateMachine,
                                       EventBus eventBus) : base(
            player, playerStateMachine, eventBus)
        {
            _characterStateMachine = playerStateMachine;
        }

        public override void EnterState()
        {
            base.EnterState();
            SetAnimationDurations();

            PlayChargeAnimation();

            _player.CharacterAttackManager.OnChargeAttackPerform += PlayPerformAttackAnimation;
            _player.CharacterAttackManager.OnBasicAttackPerform += EnterBasicAttackPerformState;
        }

        private void EnterBasicAttackPerformState()
        {
            _characterStateMachine.ChangeStateRPC((int)CharacterStateMachine.CharacterStates.AttackState);
        }

        private void SetAnimationDurations()
        {
            if (_chargeAnimationDuration == -1f)
                _chargeAnimationDuration =
                    _player.PlayerAnimationManager.GetClipLengthInSeconds(_chargeAttackAnimation);
        }

        private void PlayChargeAnimation()
        {
            _chargeAttackAnimation.SetTags(_chargeAttackAnimation.ChargeTag,
                                           _player.MainDirection.ToString(),
                                           _player.SecDirection.ToString());

            _chargeAnimationSpeed = _chargeAnimationDuration / _player.PlayerAttackManager.ChargeTime;

            _chargeAttackAnimation.ChangeAnimationSpeed(_chargeAnimationSpeed);


            _player.PlayerAnimationManager.PlayAnimation(_chargeAttackAnimation);
        }

        private void PlayPerformAttackAnimation()
        {
            _chargeAttackAnimation.ChangeAnimationSpeed(
                _player.PlayerAttackManager.AttackSpeed /
                _player.PlayerAnimationManager.GetClipLengthInSeconds(_chargeAttackAnimation));

            _chargeAttackAnimation.SetTags(_chargeAttackAnimation.PerformedTag,
                                           _player.MainDirection.ToString(),
                                           _player.SecDirection.ToString());

            _player.PlayerAnimationManager.PlayAnimation(_chargeAttackAnimation);
        }

        public override void ExitState()
        {
            base.ExitState();
            _player.CharacterAttackManager.OnChargeAttackPerform -= PlayPerformAttackAnimation;
            _player.CharacterAttackManager.OnBasicAttackPerform -= EnterBasicAttackPerformState;
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