using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Core
{
    public class CharacterStatsManager : NetworkBehaviour
    {
        [FormerlySerializedAs("_statsSO"), SerializeField]
        protected CharacterBaseStatsSO _characterStatsSO;

        public bool TryInitStats(CharacterBaseStatsSO stats, bool createNew = true)
        {
            if (_characterStatsSO != null) return false;

            if (createNew) _characterStatsSO = Instantiate(stats);
            else _characterStatsSO = stats;
            return true;
        }

        public virtual CharacterBaseStatsSO GetStats()
        {
            return _characterStatsSO;
        }
    }
}