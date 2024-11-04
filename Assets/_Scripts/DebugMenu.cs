#if UNITY_EDITOR
using System.Threading.Tasks;
using Core.InputSystem;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core
{
    public class DebugMenu : MonoBehaviour
    {
        [field:SerializeField]public  bool IsServerRunning { get; private set; }

        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private Button _spawnEnemyButton;

        [SerializeField] private GameObject _enemyObject;

        [SerializeField] private Toggle _playerInputToggleButton;

        private void Awake()
        {
            _hostButton.onClick.AddListener(StartHost);
            _clientButton.onClick.AddListener(StartClient);
            _spawnEnemyButton.onClick.AddListener(SpawnEnemy);

            NetworkManager.Singleton.OnServerStarted += OnServerStart;
        }
        private void OnServerStart()
        {
            IsServerRunning = true;
            NetworkManager.Singleton.OnServerStarted -= OnServerStart;
        }



        private void OnDestroy()
        {
            _hostButton.onClick.RemoveAllListeners();
            _clientButton.onClick.RemoveAllListeners();
            _spawnEnemyButton.onClick.RemoveAllListeners();

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
                baseControls.Gameplay.Disable();
            }
        }

        private async void StartHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        private void StartClient()
        {
            NetworkManager.Singleton.StartClient();
        }
        private async void SpawnEnemy()
        {
            if(!IsServerRunning)
            {
                StartHost();
            }

            while(!IsServerRunning)
            {
                await Task.Yield();
            }


            NetworkObject enemyNetworkObject = Instantiate(_enemyObject).GetComponent<NetworkObject>();

            enemyNetworkObject.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);

        }
    }
}
#endif