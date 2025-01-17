using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        [SerializeField] private PlayerStatsSO _playerStats;
        [Inject]
        public void Construct(PlayerStatsSO playerStats)
        {
            _playerStats = playerStats;
        }

        public override void OnNetworkSpawn()
        {
            TryInitStats(_playerStats);
        }

        public PlayerStatsSO GetPlayerStats()
        {
            return (PlayerStatsSO)_characterStatsSO;
        }
    }
}