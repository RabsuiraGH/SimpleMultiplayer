using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class CharacterAttackState : CharacterState
    {
        private readonly CharacterAttackAnimation _attackAnimation = new();

        public CharacterAttackState(CharacterManager character, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(character, stateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            PlayAttackAnimation();
        }

        private void PlayAttackAnimation()
        {
            _attackAnimation.ChangeAnimationSpeed(
                _character.CharacterAttackManager.AttackSpeed /
                _character.CharacterAnimatorManager.GetClipLengthInSeconds(_attackAnimation));

            _attackAnimation.SetTags(_character.MainDirection.ToString(), _character.SecDirection.ToString());
            _character.CharacterAnimatorManager.PlayAnimation(_attackAnimation);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            if (!_character.IsOwner) return;


            if (!_character.CharacterAttackManager.IsAttacking && !_stateMachine.IsChangingState)
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