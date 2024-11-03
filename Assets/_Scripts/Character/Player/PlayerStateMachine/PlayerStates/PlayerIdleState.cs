using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerIdleState : CharacterIdleState
    {
        private readonly PlayerManager _player;

        public PlayerIdleState(PlayerManager player, CharacterStateMachine stateMachine, EventBus eventBus) :
            base(player, stateMachine, eventBus)
        {
            _player = player;
        }

        public override void FrameUpdate()
        {
            if (!_player.IsOwner) return;

            _player.PlayerMovementManager.UpdateMovementDirectionViaInput();
            base.FrameUpdate();
        }
    }
}