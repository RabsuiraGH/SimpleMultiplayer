using System;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class Spikes : NetworkBehaviour
    {
        [SerializeField] private DamageCollider _damageCollider;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == this.gameObject) return;
            if (!other.TryGetComponent(out CharacterManager damageTarget)) return;
            if(!damageTarget.IsHost) return;

            if (damageTarget.CharacterMovementManager.IsJumping) return;

            _damageCollider.PerformDamageManually(damageTarget);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject == this.gameObject) return;
            if (!other.TryGetComponent(out CharacterManager damageTarget)) return;
            if(!damageTarget.IsHost) return;

            if (damageTarget.CharacterMovementManager.IsJumping) return;

            _damageCollider.PerformDamageManually(damageTarget);
        }
    }
}