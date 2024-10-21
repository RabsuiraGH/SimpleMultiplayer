using System;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        [Inject]
        public void Construct(PlayerStatsSO playerStats)
        {
            TryInitStats(playerStats);
        }

        public PlayerStatsSO GetPlayerStats()
        {
            return (PlayerStatsSO)_characterStatsSO;
        }
    }
}