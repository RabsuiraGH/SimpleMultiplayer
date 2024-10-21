using Core.InputSystem;
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