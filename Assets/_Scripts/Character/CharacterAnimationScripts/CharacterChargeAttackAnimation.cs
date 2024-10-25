using Core.CustomAnimationSystem;
using UnityEngine;

namespace Core
{
    public class CharacterChargeAttackAnimation : CustomAnimationBase
    {
        public override float AnimationSpeed {  get; protected set; } = 1f;
        public override string AnimationName { get; protected set; } = "ChargeAttack";
        protected override string AnimationAdditionalTag { get; set; } = string.Empty;

        public readonly string ChargeTag = "Charge";
        public readonly string IdleTag = "Idle";
        public readonly string PerformedTag = "Perform";
    }
}