using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "DamageEffectSO", menuName = "Game/Effects/DamageEffectSO")]
    public class CharacterDamageEffectSO : InstantEffectSO
    {
        public float Damage;

        public override void ProcessEffect(CharacterManager character)
        {
            if (character == null || !character.IsOwner) return;

            character.CharacterStatsManager.GetStats().Health.CurrentValue.Value -= Damage;
        }
    }
}