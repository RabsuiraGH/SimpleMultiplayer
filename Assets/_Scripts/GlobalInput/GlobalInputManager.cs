using Core.GameEventSystem;
using Core.InputSystem;
using Core.Utility.DebugTool;
using UnityEngine;
using Zenject;

using static Core.Utility.DebugTool.DebugColorOptions.HtmlColor;

namespace Core
{
    public class GlobalInputManager : MonoBehaviour
    {
        [SerializeField] private BaseControls _baseControls = null;

        [SerializeField] private EventBus _eventBus = null;

        [SerializeField] private DebugLogger _debugger = new();

#if UNITY_EDITOR
        [SerializeField] private InputTypeDebug _startInputModeDebug;
#endif

        [Inject]
        public void Construct(EventBus eventBus, BaseControls baseInput)
        {
            _eventBus = eventBus;
            _baseControls = baseInput;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            // SUBSCRIBE OWN METHODS TO CHANGE UI/GAMEPLAY ETC INPUTS

            //_eventBus.Subscribe<SwitchToUIInputSignal>(SwitchToUIInput);
            //_eventBus.Subscribe<SwitchToGameplayInputSignal>(SwithToGameplayInput);
        }

#if UNITY_EDITOR

        [EasyButtons.Button]
        public void DEBUG_EnableControls(bool enable = true)
        {
            if (enable)
                _baseControls.Enable();
            else
                _baseControls.Disable();
        }

        private void Update()
        {
            _debugger.Log(this, ($"Innput State: Global: {(_baseControls.Global.enabled).Color(Bool)}  Gameplay: {(_baseControls.Gameplay.enabled).Color(Bool)}  UI: {(_baseControls.UI.enabled).Color(Bool)}"));
        }

        private enum InputTypeDebug
        {
            Gameplay,
            UI,
        }

        [EasyButtons.Button]
        private void SwitchInputDebug(InputTypeDebug type)
        {
            switch (type)
            {
                case InputTypeDebug.Gameplay:
                    //SwithToGameplayInput(null);
                    break;

                case InputTypeDebug.UI:
                    //SwitchToUIInput(null);
                    break;

                default:
                    break;
            }
        }

#endif
    }
}