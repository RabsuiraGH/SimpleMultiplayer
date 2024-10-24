using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Core
{
    public class CharacterEffectsManager : NetworkBehaviour
    {
        public static void ProcessInstantEffect(InstantEffectSO effect, CharacterManager target)
        {
            effect.ProcessEffect(target);
        }

        public bool TryProcessInstantEffect(InstantEffectSO effect)
        {
            if (!TryGetComponent(out CharacterManager thisCharacter)) return false;
            effect.ProcessEffect(thisCharacter);
            return true;
        }
    }
}