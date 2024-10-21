using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerZInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInputManager _playerInputManager;

        [SerializeField] private PlayerStatsSO _playerBaseStats;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInputManager>().FromInstance(_playerInputManager);
            Container.Bind<PlayerStatsSO>().FromInstance(_playerBaseStats);
        }
    }
}