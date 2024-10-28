using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "DamageEffectSO", menuName = "Game/Effects/DamageEffectSO")]
    public class CharacterDamageEffectSO : InstantEffectSO
    {
        public float Damage;

        public override void ProcessEffect(CharacterManager character)
        {
            if (character == null) return;
            if (!character.TryGetComponent(out IDamageable damageable)) return;
            damageable.GetDamage(Damage);
        }
    }
}