using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class Spikes : NetworkBehaviour
    {
        [SerializeField] protected CircleCollider2D _collider;
        [SerializeField] private LayerMask _feetLayer;

        [SerializeField] private CharacterDamageEffectSO _damageEffectOrigin;
        [SerializeField] private CharacterDamageEffectSO _damageEffect;

        [SerializeField] private List<CharacterManager> _onSpikes;

        protected virtual void Awake()
        {
            _damageEffect = Instantiate(_damageEffectOrigin);
            _collider = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _feetLayer) == 0) return;

            if (collision.gameObject == this.gameObject) return;
            CharacterManager damageTarget = collision.GetComponentInParent<CharacterManager>();
            if (damageTarget == null || !damageTarget.IsHost) return;

            if (!_onSpikes.Contains(damageTarget))
            {
                damageTarget.CharacterMovementManager.OnJump += ResetSpikeImmunity;
                _onSpikes.Add(damageTarget);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _feetLayer) == 0) return;

            if (collision.gameObject == this.gameObject) return;
            CharacterManager damageTarget = collision.GetComponentInParent<CharacterManager>();
            if (damageTarget == null || !damageTarget.IsHost) return;

            if (_onSpikes.Contains(damageTarget))
            {
                damageTarget.CharacterMovementManager.OnJump -= ResetSpikeImmunity;
                _onSpikes.Remove(damageTarget);
            }
        }

        private void ResetSpikeImmunity(CharacterManager character)
        {
            if (_onSpikes.Contains(character)) return;
            _onSpikes.Add(character);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            for (int index = 0; index < _onSpikes.Count; index++)
            {
                CharacterManager damageTarget = _onSpikes[index];
                if (!damageTarget.CharacterMovementManager.IsJumping)
                {
                    CharacterEffectsManager.ProcessInstantEffect(_damageEffect, damageTarget);
                    _onSpikes.Remove(damageTarget);
                    index--;
                }
            }
        }
    }
}