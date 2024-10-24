using R3;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterStatsManager : NetworkBehaviour
    {
        [SerializeField] protected CharacterBaseStatsSO _characterStatsSO;

        [SerializeField] public NetworkVariable<float> Health;

        public bool TryInitStats(CharacterBaseStatsSO stats, bool createNew = true)
        {
            if (_characterStatsSO != null)
            {
                return false;
            }

            if (createNew)
            {
                _characterStatsSO = Instantiate(stats);
            }
            else
            {
                _characterStatsSO = stats;
            }

            // send request to change stat on server,
            Health.OnValueChanged += _characterStatsSO.Health.ChangeCurrent;


            return true;
        }

        public virtual CharacterBaseStatsSO GetStats()
        {
            return _characterStatsSO;
        }
    }
}