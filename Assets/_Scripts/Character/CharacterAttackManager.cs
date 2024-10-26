using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterAttackManager : NetworkBehaviour
    {
        [SerializeField] private CharacterManager _character;
        [SerializeField] protected DamageCollider _damageCollider;

        [field: SerializeField] public bool IsAttacking { get; private set; } = false;
        [field: SerializeField] public bool IsCharging { get; private set; } = false;

        [field: SerializeField] public bool AttackCharged { get; private set; } = false;

        [SerializeField] protected float _pushDistance;
        [SerializeField] protected float _pushTime;

        public event Action OnAttackStart;
        public event Action OnChargeAttackCharge;
        public event Action OnChargeAttackPerform;
#if UNITY_EDITOR
        private Vector3 _attackPoint = Vector3.zero;
#endif
        protected virtual void Awake()
        {
            _character = GetComponent<CharacterManager>();
        }

        public void StopAttackState()
        {
            IsAttacking = false;
            IsCharging = false;
            AttackCharged = false;
            _damageCollider.gameObject.SetActive(false);
        }

        [ServerRpc]
        protected void PerformAttackServerRPC(float attackPointX, float attackPointY)
        {
            PerformAttackClientRpc(attackPointX, attackPointY);
        }

        [ClientRpc]
        protected void PerformAttackClientRpc(float attackPointX, float attackPointY)
        {
            StopAllCoroutines();
            IsAttacking = true;

            Vector2 position = this.transform.position;
            Vector2 mousePosition = new Vector2(attackPointX, attackPointY);
            Vector2 attackDirection = (mousePosition - position).normalized;


            // change direction of character before invoking anything that can play animation
            _character.ChangeFaceDirectionViaMovement(attackDirection);

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
            if (IsAttacking) return;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(this.transform.position, _attackPoint);
        }
#endif
        private IEnumerator ChargeAttackTimer()
        {
            float attackSpeed = _character.CharacterStatsManager.GetStats().AttackSpeed.CurrentValueReadonly
                                          .CurrentValue;
            float chargeTime = _character.CharacterStatsManager.GetStats().ChargeAttackTime.CurrentValueReadonly
                                         .CurrentValue;

            float timer = 0f;
            while (timer < chargeTime)
            {
                timer += Time.deltaTime * attackSpeed;
                yield return null;
            }

            AttackCharged = true;
        }

        public void StartChargeAttackCharge()
        {
            IsCharging = true;
            OnChargeAttackCharge?.Invoke(); // change state

            StopAllCoroutines();
            StartCoroutine(ChargeAttackTimer());
        }

        public void TryPerformChargeAttack()
        {
            if (!AttackCharged) return;

            IsAttacking = true;
            IsCharging = false;
            OnChargeAttackPerform?.Invoke();
        }

        public void CancelChargeAttack()
        {
            IsCharging = false;
            AttackCharged = false;
        }
    }
}