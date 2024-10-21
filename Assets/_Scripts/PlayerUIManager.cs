using R3;
using UnityEngine;

namespace Core
{
    public class PlayerUIManager : MonoBehaviour
    {
        [field: SerializeField] public FillBarUI PlayerHealthBar { get; private set; }

        public void SetupUI(PlayerStatsManager playerStatsManager)
        {
            CharacterBaseStatsSO stats = playerStatsManager.GetStats();

            Observable.CombineLatest(stats.Health.CurrentValueReadonly, stats.Health.MaxValueReadonly)
                      .Subscribe(values => PlayerHealthBar.UpdateValue(values[0], values[1]));
        }
    }
}