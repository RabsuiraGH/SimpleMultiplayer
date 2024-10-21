#if UNITY_EDITOR
using Core.InputSystem;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core
{
    public class DebugMenu : MonoBehaviour
    {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;

        [SerializeField] private Toggle _playerInputToggleButton;

        private void Awake()
        {
            _hostButton.onClick.AddListener(StartHost);
            _clientButton.onClick.AddListener(StartClient);
        }

        private void OnDestroy()
        {
            _hostButton.onClick.RemoveAllListeners();
            _clientButton.onClick.RemoveAllListeners();
        }


        [Inject]
        public void Construct(BaseControls baseControls)
        {
            _playerInputToggleButton.onValueChanged.AddListener(enable => TogglePlayerInput(baseControls, enable));
            _playerInputToggleButton.SetIsOnWithoutNotify(baseControls.Gameplay.enabled);
        }

        private void TogglePlayerInput(BaseControls baseControls, bool enable)
        {
            if (enable)
            {
                baseControls.Gameplay.Enable();
            }
            else
            {
                baseControls.Gameplay.Enable();
            }
        }

        private void StartHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        private void StartClient()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
#endif