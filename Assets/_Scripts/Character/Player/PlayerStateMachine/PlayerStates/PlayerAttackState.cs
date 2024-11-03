using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerAttackState : CharacterAttackState
    {
        private readonly PlayerManager _player;

        public PlayerAttackState(PlayerManager player, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(player, stateMachine, eventBus)
        {
            _player = player;
        }
    }
}