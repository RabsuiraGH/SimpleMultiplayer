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
        [SerializeField] protected SingleDamageCollider SingleDamageCollider;
        [SerializeField] protected SingleDamageCollider ChargeSingleDamageCollider;

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
        private Vector3 d_attackPoint = Vector3.zero;
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

        // Stop attack methods
        private void StopAttackState()
        {
            IsAttacking = false;
            IsCharging = false;
            AttackCharged = false;
            SingleDamageCollider.gameObject.SetActive(false);
            ChargeSingleDamageCollider.gameObject.SetActive(false);
        }

        public void StopAttackStateRpc()
        {
            if (!IsOwner) return;

            StopAttackState();


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
            if (IsOwner) return;
            StopAttackState();
        }

        // Basic attack methods
        protected void PerformBasicAttack(float attackPointX, float attackPointY)
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
            d_attackPoint = mousePosition;
#endif

            SingleDamageCollider.transform.localPosition = attackDirection;

            if (IsOwner)
            {
                StartCoroutine(_character.CharacterMovementManager.MovePositionOverTime(
                                   position, position + attackDirection * _pushDistance, _pushTime));
            }


            if (IsHost)
            {
                SingleDamageCollider.gameObject.SetActive(true);
            }
        }

        [ServerRpc]
        protected void PerformBasicAttackServerRPC(float attackPointX, float attackPointY)
        {
            PerformBasicAttackClientRpc(attackPointX, attackPointY);
        }

        [ClientRpc]
        protected void PerformBasicAttackClientRpc(float attackPointX, float attackPointY)
        {
            if (IsOwner) return;
            PerformBasicAttack(attackPointX, attackPointY);
        }

        // Charge attack methods
        protected void PerformChargeAttack(float attackPointX, float attackPointY)
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
            d_attackPoint = mousePosition;
#endif

            if (IsOwner)
            {
                StartCoroutine(_character.CharacterMovementManager.MovePositionOverTime(
                                   position, position + attackDirection * _chargePushDistance, _chargePushTime));
            }

            if (IsHost)
            {
                ChargeSingleDamageCollider.gameObject.SetActive(true);
            }
        }

        [ServerRpc]
        protected void PerformChargeAttackServerRPC(float attackPointX, float attackPointY)
        {
            PerformChargeAttackClientRpc(attackPointX, attackPointY);
        }

        [ClientRpc]
        protected void PerformChargeAttackClientRpc(float attackPointX, float attackPointY)
        {
            if (IsOwner) return;
            PerformChargeAttack(attackPointX, attackPointY);
        }

        public virtual void PerformAttackRpc()
        {
            if (IsAttacking || _character.IsPerformingMainAction) return;
            IsCharging = false;
            _character.IsPerformingMainAction = true;
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(this.transform.position, d_attackPoint);
        }
#endif
        private IEnumerator ChargeAttackTimer()
        {
            float timer = 0f;
            while (timer < ChargeTime)
            {
                if (!IsCharging) yield break;
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