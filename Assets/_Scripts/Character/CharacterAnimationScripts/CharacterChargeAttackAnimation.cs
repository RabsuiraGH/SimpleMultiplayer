using Core.CustomAnimationSystem;
using UnityEngine;

namespace Core
{
    public class CharacterChargeAttackAnimation : CustomAnimationBase
    {
        public override float AnimationSpeed {  get; protected set; } = 1f;
        public override string AnimationName { get; protected set; } = "ChargeAttack";
        protected override string AnimationAdditionalTag { get; set; } = string.Empty;

        public const string CHARGE_TAG = "Charge";
        public const string IDLE_TAG = "Idle";
        public const string PERFORMED_TAG = "Perform";
    }
}