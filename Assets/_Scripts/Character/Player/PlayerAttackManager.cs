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
            if (IsAttacking) return;

            Vector2 mouse = Directions.GetDirectionsViaMouse(_camera, transform.position, out _, out _);

            if (IsHost)
            {
                PerformAttackClientRpc(mouse.x, mouse.y);
            }
            else
            {
                PerformAttackServerRPC(mouse.x, mouse.y);
            }
        }
    }
}