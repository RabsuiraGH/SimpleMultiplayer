using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerChargeAttackState : CharacterChargeAttackState
    {
        private readonly PlayerManager _player;

        public PlayerChargeAttackState(PlayerManager player, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(player, stateMachine, eventBus)
        {
            _player = player;
        }
    }
}