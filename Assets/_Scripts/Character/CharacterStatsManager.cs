using R3;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class CharacterStatsManager : NetworkBehaviour, IDamageable
    {
        [SerializeField] protected CharacterBaseStatsSO _characterStatsSO;

        [SerializeField] public NetworkVariable<float> Health;
        [SerializeField] public NetworkVariable<float> MaxHealth;

        [SerializeField] public NetworkVariable<float> Damage;
        [SerializeField] public NetworkVariable<float> MaxDamage;

        [SerializeField] public NetworkVariable<float> Armor;
        [SerializeField] public NetworkVariable<float> MaxArmor;

        [SerializeField] public NetworkVariable<float> MovementSpeed;
        [SerializeField] public NetworkVariable<float> MaxMovementSpeed;

        [SerializeField] public NetworkVariable<float> AttackSpeed;
        [SerializeField] public NetworkVariable<float> MaxAttackSpeed;

        [SerializeField] public NetworkVariable<float> ChargeAttackTime;
        [SerializeField] public NetworkVariable<float> MaxChargeAttackTime;

        protected bool TryInitStats(CharacterBaseStatsSO stats, bool createNew = true)
        {
            if (_characterStatsSO != null)
            {
                return false;
            }

            _characterStatsSO = createNew ? Instantiate(stats) : stats;

            SubscribeToStatChanges(Health, MaxHealth, _characterStatsSO.Health);
            SubscribeToStatChanges(Damage, MaxDamage, _characterStatsSO.Damage);
            SubscribeToStatChanges(Armor, MaxArmor, _characterStatsSO.Armor);
            SubscribeToStatChanges(MovementSpeed, MaxMovementSpeed, _characterStatsSO.MovementSpeed);
            SubscribeToStatChanges(AttackSpeed, MaxAttackSpeed, _characterStatsSO.AttackSpeed);
            SubscribeToStatChanges(ChargeAttackTime, MaxChargeAttackTime, _characterStatsSO.ChargeAttackTime);

            return true;
        }

        private void SubscribeToStatChanges(NetworkVariable<float> current,
                                            NetworkVariable<float> max,
                                            StatParameter statParameter)
        {
            if (IsHost)
            {
                current.Initialize(this);
                max.Initialize(this);

                current.Value = statParameter.CurrentValue.Value;
                max.Value = statParameter.MaxValue.Value;
            }


            current.OnValueChanged += statParameter.ChangeCurrent;
            max.OnValueChanged += statParameter.ChangeMaximum;



        }

        private void UnsubscribeFromStatChanges(NetworkVariable<float> current,
                                                NetworkVariable<float> max,
                                                StatParameter statParameter)
        {
            current.OnValueChanged -= statParameter.ChangeCurrent;
            max.OnValueChanged -= statParameter.ChangeMaximum;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (_characterStatsSO != null)
            {
                UnsubscribeFromStatChanges(Health, MaxHealth, _characterStatsSO.Health);
                UnsubscribeFromStatChanges(Damage, MaxDamage, _characterStatsSO.Damage);
                UnsubscribeFromStatChanges(Armor, MaxArmor, _characterStatsSO.Armor);
                UnsubscribeFromStatChanges(MovementSpeed, MaxMovementSpeed, _characterStatsSO.MovementSpeed);
                UnsubscribeFromStatChanges(AttackSpeed, MaxAttackSpeed, _characterStatsSO.AttackSpeed);
                UnsubscribeFromStatChanges(ChargeAttackTime, MaxChargeAttackTime, _characterStatsSO.ChargeAttackTime);
            }
        }

        public virtual CharacterBaseStatsSO GetStats()
        {
            return _characterStatsSO;
        }

        public void GetDamage(float initialDamage)
        {
                float resultDamage = initialDamage - Armor.Value;
                Health.Value -= resultDamage;
        }
    }
}