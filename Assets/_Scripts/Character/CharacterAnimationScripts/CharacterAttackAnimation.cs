using Core.CustomAnimationSystem;
using UnityEngine;

namespace Core
{
    public class CharacterAttackAnimation : CustomAnimationBase
    {
        public override float AnimationSpeed { get; protected set; } = 1f;
        public override string AnimationName { get; protected set; } = "Attack";
        protected override string AnimationAdditionalTag { get; set; } = string.Empty;
    }
}