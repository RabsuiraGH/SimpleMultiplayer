using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerZInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInputManager _playerInputManager;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInputManager>().FromInstance(_playerInputManager);
        }
    }
}