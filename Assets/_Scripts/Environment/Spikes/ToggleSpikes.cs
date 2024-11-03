using System;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class ToggleSpikes : Spikes
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _closeStateSprite;
        [SerializeField] private Sprite _openStateSprite;

        [SerializeField] protected bool _isClosed = false;

        protected override void Awake()
        {
            base.Awake();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ToggleSpikesRPC(bool open)
        {
            if (!IsOwner) return;
            if (open)
                OpenSpikes();
            else
                CloseSpikes();


            if (IsHost)
                ToggleSpikesClientRPC(open);
            else
                ToggleSpikesServerRPC(open);
        }

        protected virtual void CloseSpikes()
        {
            _spriteRenderer.sprite = _closeStateSprite;
            _isClosed = true;
            _collider.enabled = false;
        }

        protected virtual void OpenSpikes()
        {
            _spriteRenderer.sprite = _openStateSprite;
            _isClosed = false;
            _collider.enabled = true;
        }

        [ServerRpc]
        private void ToggleSpikesServerRPC(bool open)
        {
            ToggleSpikesClientRPC(open);
        }

        [ClientRpc]
        private void ToggleSpikesClientRPC(bool open)
        {
            if (IsOwner) return;
            if (open)
                OpenSpikes();
            else
                CloseSpikes();
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_spriteRenderer == null) return;
            ToggleSpikesRPC(!_isClosed);
        }
#endif
    }
}