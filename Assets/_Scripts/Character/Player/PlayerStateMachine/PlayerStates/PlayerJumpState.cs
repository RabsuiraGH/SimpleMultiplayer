using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerJumpState : CharacterJumpState
    {
        private readonly PlayerManager _player;

        public PlayerJumpState(PlayerManager player, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(player, stateMachine, eventBus)
        {
            _player = player;
        }
    }
}