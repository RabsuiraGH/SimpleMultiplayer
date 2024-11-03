using Core.GameEventSystem;
using UnityEngine;

namespace Core
{
    public class PlayerMovementState : CharacterMovementState
    {
        private readonly PlayerManager _player;

        public PlayerMovementState(PlayerManager player, CharacterStateMachine stateMachine, EventBus eventBus) :
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