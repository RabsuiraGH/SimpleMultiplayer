using System;
using System.Collections;
using R3;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterAttackManager : NetworkBehaviour
    {
        [SerializeField] private CharacterManager _character;
        [SerializeField] protected DamageCollider _damageCollider;
        [SerializeField] protected DamageCollider _chargeDamageCollider;

        [field: SerializeField] public bool IsAttacking { get; private set; } = false;
        [field: SerializeField] public bool IsCharging { get; private set; } = false;

        [field: SerializeField] public bool AttackCharged { get; private set; } = false;

        [field: SerializeField] public float AttackSpeed { get; private set; }
        [field: SerializeField] public float ChargeTime { get; private set; }

        [SerializeField] protected float _pushDistance;
        [SerializeField] protected float _pushTime;
        [SerializeField] protected float _chargePushDistance;
        [SerializeField] protected float _chargePushTime;

        public event Action OnBasicAttackPerform;
        public event Action OnChargeAttackCharge;
        public event Action OnChargeAttackPerform;
#if UNITY_EDITOR
        private Vector3 _attackPoint = Vector3.zero;
#endif
        protected virtual void Awake()
        {
            _character = GetComponent<CharacterManager>();
        }

        private void Start()
        {
            _character.CharacterStatsManager.GetStats().AttackSpeed.CurrentValueReadonly
                      .Subscribe(val => AttackSpeed = val);

            _character.CharacterStatsManager.GetStats().ChargeAttackTime.CurrentValueReadonly
                      .Subscribe(val => ChargeTime = val);
        }

        public void StopAttackState()
        {
            if (IsHost)
            {
                StopAttackStateClientRpc();
            }
            else
            {
                StopAttackStateServerRpc();
            }
        }

        [ServerRpc]
        protected void StopAttackStateServerRpc()
        {
            StopAttackStateClientRpc();
        }

        [ClientRpc]
        protected void StopAttackStateClientRpc()
        {
            IsAttacking = false;
            IsCharging = false;
            AttackCharged = false;
            _damageCollider.gameObject.SetActive(false);
            _chargeDamageCollider.gameObject.SetActive(false);
        }

        [ServerRpc]
        protected void PerformBasicAttackServerRPC(float attackPointX, float attackPointY)
        {
            PerformBasicAttackClientRpc(attackPointX, attackPointY);
        }

        [ClientRpc]
        protected void PerformBasicAttackClientRpc(float attackPointX, float attackPointY)
        {
            StopAllCoroutines();
            IsAttacking = true;

            Vector2 position = this.transform.position;
            Vector2 mousePosition = new Vector2(attackPointX, attackPointY);
            Vector2 attackDirection = (mousePosition - position).normalized;


            // change direction of character before invoking anything that can play animation
            _character.ChangeFaceDirectionViaMovement(attackDirection);

            OnBasicAttackPerform?.Invoke();

#if UNITY_EDITOR
            _attackPoint = mousePosition;
#endif

            StartCoroutine(_character.CharacterMovementManager.MovePositionOverTime(
                               position, position + attackDirection * _pushDistance, _pushTime));

            _damageCollider.transform.localPosition = attackDirection;
            _damageCollider.gameObject.SetActive(true);
        }

        [ServerRpc]
        protected void PerformChargeAttackServerRPC(float attackPointX, float attackPointY)
        {
            PerformChargeAttackClientRpc(attackPointX, attackPointY);
        }

        [ClientRpc]
        protected void PerformChargeAttackClientRpc(float attackPointX, float attackPointY)
        {
            StopAllCoroutines();
            IsAttacking = true;

            Vector2 position = this.transform.position;
            Vector2 mousePosition = new Vector2(attackPointX, attackPointY);
            Vector2 attackDirection = (mousePosition - position).normalized;


            // change direction of character before invoking anything that can play animation
            _character.ChangeFaceDirectionViaMovement(attackDirection);

            OnChargeAttackPerform?.Invoke();

#if UNITY_EDITOR
            _attackPoint = mousePosition;
#endif

            StartCoroutine(_character.CharacterMovementManager.MovePositionOverTime(
                               position, position + attackDirection * _chargePushDistance, _chargePushTime));
            _chargeDamageCollider.gameObject.SetActive(true);
        }

        public virtual void PerformAttack()
        {
            if (IsAttacking) return;
            IsCharging = false;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(this.transform.position, _attackPoint);
        }
#endif
        private IEnumerator ChargeAttackTimer()
        {
            float timer = 0f;
            while (timer < ChargeTime)
            {
                timer += Time.deltaTime * AttackSpeed;
                yield return null;
            }

            AttackCharged = true;
        }

        public void StartChargeAttackCharge()
        {
            if (IsAttacking) return;
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