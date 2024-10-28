using R3;
using UnityEngine;

namespace Core
{
    public class PlayerTeamUIManager : MonoBehaviour
    {
        [field: SerializeField] public FillBarUI PlayerHealthBarTeam { get; private set; }

        public void SetupTeamUI(PlayerStatsManager playerStatsManager)
        {

            CharacterBaseStatsSO stats = playerStatsManager.GetStats();

            Observable.CombineLatest(stats.Health.CurrentValueReadonly, stats.Health.MaxValueReadonly)
                      .Subscribe(values => PlayerHealthBarTeam.UpdateValue(values[0], values[1]));

            PlayerHealthBarTeam.gameObject.SetActive(true);
        }
    }
}