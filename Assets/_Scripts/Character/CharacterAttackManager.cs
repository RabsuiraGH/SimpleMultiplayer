using System;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterAttackManager : NetworkBehaviour
    {
        [SerializeField] private CharacterManager _character;
        [SerializeField] private Camera _camera;
        [SerializeField] private DamageCollider _damageCollider;

        [field: SerializeField] public bool IsAttacking { get; private set; } = false;

        [SerializeField] private float _pushDistance;
        [SerializeField] private float _pushTime;

        public event Action OnAttackStart;

#if UNITY_EDITOR
        private Vector3 _attackPoint = Vector3.zero;
#endif
        protected virtual void Awake()
        {
            _camera = Camera.main;
            _character = GetComponent<CharacterManager>();
        }

        public void StopAttackState()
        {
            if (IsAttacking)
            {
                IsAttacking = false;
            }

            _damageCollider.gameObject.SetActive(false);
        }

        [ServerRpc]
        private void PerformAttackServerRPC(float mouseX, float mouseY)
        {
            PerformAttackClientRpc(mouseX, mouseY);
        }

        [ClientRpc]
        private void PerformAttackClientRpc(float mouseX, float mouseY)
        {
            if (IsAttacking) return;

            StopAllCoroutines();
            IsAttacking = true;

            Vector2 position = this.transform.position;
            Vector2 mousePosition = new Vector2(mouseX, mouseY);
            Vector2 attackDirection = (mousePosition - position).normalized;


            // change direction of character before invoking anything that can play animation
            _character.ChangeFaceDirection(attackDirection);

            OnAttackStart?.Invoke();

#if UNITY_EDITOR
            _attackPoint = mousePosition;
#endif

            StartCoroutine(_character.CharacterMovementManager.MovePositionOverTime(
                               position, position + attackDirection * _pushDistance, _pushTime));

            _damageCollider.transform.localPosition = attackDirection;
            _damageCollider.gameObject.SetActive(true);
        }

        public virtual void PerformAttack()
        {
            Vector2 mouse = Directions.GetDirectionsViaMouse(_camera, transform.position, out _, out _);
            if (IsHost)
            {
                PerformAttackClientRpc(mouse.x, mouse.y);
            }
            else
            {
                PerformAttackServerRPC(mouse.x, mouse.y);
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(this.transform.position, _attackPoint);
        }
#endif
    }
}