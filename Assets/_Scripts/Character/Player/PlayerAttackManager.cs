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

        public override void PerformAttack()
        {
            if (IsAttacking || _player.IsPerformingMainAction) return;
            Vector2 mouse = Directions.GetDirectionsViaMouse(_camera, transform.position, out _, out _);
            _player.IsPerformingMainAction = true;

            if (IsHost)
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
            else
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