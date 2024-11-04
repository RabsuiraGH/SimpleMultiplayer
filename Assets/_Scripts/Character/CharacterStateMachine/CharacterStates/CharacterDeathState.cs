using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class CharacterDeathState : CharacterState
    {
        protected readonly CharacterDeathAnimation _deathAnimation = new();

        public CharacterDeathState(CharacterManager character, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(character, stateMachine, eventBus)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            PlayDeathAnimation();
        }

        protected virtual void PlayDeathAnimation()
        {
            _deathAnimation.SetTags(CharacterDeathAnimation.DEATH_TYPE);

            _character.CharacterAnimatorManager.PlayAnimation(_deathAnimation);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            if (!_character.IsOwner) return;
            if (!_character.CharacterDeathManager.IsDead && !_stateMachine.IsChangingState)
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