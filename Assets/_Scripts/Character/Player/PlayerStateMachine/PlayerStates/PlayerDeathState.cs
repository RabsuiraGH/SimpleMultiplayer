using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerDeathState : CharacterDeathState
    {
        public PlayerDeathState(CharacterManager character, CharacterStateMachine stateMachine, EventBus eventBus) : base(character, stateMachine, eventBus)
        {
        }
        protected override void PlayDeathAnimation()
        {
            if (Random.Range(0, 2) == 0)
            {
                _deathAnimation.SetTags(PlayerDeathAnimation.DEATH_TYPE);
            }
            else
            {
                _deathAnimation.SetTags(PlayerDeathAnimation.DEATH_TYPE_SOUL);
            }


            _character.CharacterAnimatorManager.PlayAnimation(_deathAnimation);
        }
    }
}