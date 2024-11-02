using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    public class DamageCollider : MonoBehaviour
    {
        [SerializeField] private bool _auto = true;
        [SerializeField] private CharacterDamageEffectSO _damageEffectOrigin;
        [SerializeField] private CharacterDamageEffectSO _damageEffect;

        [SerializeField] private List<CharacterManager> _alreadyDamaged;

        private void Awake()
        {
            _damageEffect = Instantiate(_damageEffectOrigin);
        }

        public void PerformDamageManually(CharacterManager damageTarget)
        {
            if(!damageTarget.IsHost) return;
            if (_alreadyDamaged.Contains(damageTarget)) return;

            CharacterEffectsManager.ProcessInstantEffect(_damageEffectOrigin, damageTarget);
            _alreadyDamaged.Add(damageTarget);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!_auto) return;
            if (other.gameObject == this.gameObject) return;
            if (!other.TryGetComponent(out CharacterManager damageTarget)) return;
            if(!damageTarget.IsHost) return;
            if (_alreadyDamaged.Contains(damageTarget)) return;

            CharacterEffectsManager.ProcessInstantEffect(_damageEffectOrigin, damageTarget);
            _alreadyDamaged.Add(damageTarget);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out CharacterManager damageTarget)) return;

            if (_alreadyDamaged.Contains(damageTarget)) _alreadyDamaged.Remove((damageTarget));
        }
    }
}