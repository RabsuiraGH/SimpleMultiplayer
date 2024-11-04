using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class CharacterChargeAttackState : CharacterState
    {
        private readonly CharacterChargeAttackAnimation _chargeAttackAnimation = new();

        private float _chargeAnimationDuration = -1f;
        private float _chargeAnimationSpeed;

        public CharacterChargeAttackState(CharacterManager character, CharacterStateMachine playerStateMachine,
                                             EventBus eventBus) : base(
            character, playerStateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            SetAnimationDurations();

            PlayChargeAnimation();

            _character.CharacterAttackManager.OnChargeAttackPerform += PlayPerformAttackAnimation;
            _character.CharacterAttackManager.OnBasicAttackPerform += EnterBasicAttackPerformState;
        }

        private void EnterBasicAttackPerformState()
        {
            _characterStateMachine.ChangeStateRPC(_characterStateMachine.AttackState);
        }

        private void SetAnimationDurations()
        {
            if (_chargeAnimationDuration == -1f)
                _chargeAnimationDuration =
                    _character.CharacterAnimatorManager.GetClipLengthInSeconds(_chargeAttackAnimation);
        }

        private void PlayChargeAnimation()
        {
            _chargeAttackAnimation.SetTags(CharacterChargeAttackAnimation.CHARGE_TAG,
                                           _character.MainDirection.ToString(),
                                           _character.SecDirection.ToString());

            _chargeAnimationSpeed = _chargeAnimationDuration / _character.CharacterAttackManager.ChargeTime;

            _chargeAttackAnimation.ChangeAnimationSpeed(_chargeAnimationSpeed);


            _character.CharacterAnimatorManager.PlayAnimation(_chargeAttackAnimation);
        }

        private void PlayPerformAttackAnimation()
        {
            _chargeAttackAnimation.ChangeAnimationSpeed(
                _character.CharacterAttackManager.AttackSpeed /
                _character.CharacterAnimatorManager.GetClipLengthInSeconds(_chargeAttackAnimation));

            _chargeAttackAnimation.SetTags(CharacterChargeAttackAnimation.PERFORMED_TAG,
                                           _character.MainDirection.ToString(),
                                           _character.SecDirection.ToString());

            _character.CharacterAnimatorManager.PlayAnimation(_chargeAttackAnimation);
        }

        public override void ExitState()
        {
            base.ExitState();
            _character.CharacterAttackManager.OnChargeAttackPerform -= PlayPerformAttackAnimation;
            _character.CharacterAttackManager.OnBasicAttackPerform -= EnterBasicAttackPerformState;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if (!_character.CharacterAttackManager.IsAttacking && !_stateMachine.IsChangingState)
            {
                if (!_character.CharacterAttackManager.IsCharging)
                {
                    _stateMachine.ChangeStateRPC(_characterStateMachine.IdleState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}