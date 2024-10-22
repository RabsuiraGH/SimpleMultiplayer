using UnityEngine;

namespace Core
{
    public abstract class InstantEffectSO : ScriptableObject
    {
        public abstract void ProcessEffect(CharacterManager character);
    }
}