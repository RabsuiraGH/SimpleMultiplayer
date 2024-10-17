using Core.InputSystem;
using UnityEngine.EventSystems;
using Zenject;

namespace Core
{
    public class InputZInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BaseControls>().AsSingle().NonLazy();
        }
    }
}