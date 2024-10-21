using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    [CreateAssetMenu(fileName = "CharacterBaseStatsSO", menuName = "Game/Stats/CharacterBaseStatsSO")]
    public class CharacterBaseStatsSO : ScriptableObject
    {
        [field: SerializeField] public StatParameter Health { get; protected set; } = new(0, 20);
        [field: SerializeField] public StatParameter Damage { get; protected set; } = new(1, 5 );
        [field: SerializeField] public StatParameter Armor { get; protected set; } = new(-100, 0);
        [field: SerializeField] public StatParameter MovementSpeed { get; protected set; } = new(0.1f, 2f);
    }
}