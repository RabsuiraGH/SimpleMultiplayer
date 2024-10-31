using UnityEngine;

namespace Core
{
    public class PlayerAttackManager : CharacterAttackManager
    {
        [SerializeField] private PlayerManager _player;
        [SerializeField] private Camera _camera;

        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;

            _player = GetComponent<PlayerManager>();
        }

        public override void PerformAttackRpc()
        {
            if (!IsOwner) return; // We want to perform it ONLY on our client
            if (IsAttacking || _player.IsPerformingMainAction) return;

            Vector2 mouse = Directions.GetDirectionsViaMouse(_camera, transform.position, out _, out _);

            _player.IsPerformingMainAction = true;

            // Perform on our owner client side
            if (!AttackCharged)
            {
                PerformBasicAttack(mouse.x, mouse.y);
            }
            else
            {
                PerformChargeAttack(mouse.x, mouse.y);
            }

            // Now we must notify other clients about our attack
            if (IsHost) // If we are host, we must send our attack to other clients
            {
                if (!AttackCharged)
                {
                    PerformBasicAttackClientRpc(mouse.x, mouse.y);
                }
                else
                {
                    PerformChargeAttackClientRpc(mouse.x, mouse.y);
                }
            }
            else // If we are not host, we must send our attack to host to perform on other clients
            {
                if (!AttackCharged)
                {
                    PerformBasicAttackServerRPC(mouse.x, mouse.y);
                }
                else
                {
                    PerformChargeAttackServerRPC(mouse.x, mouse.y);
                }
            }
        }
    }
}