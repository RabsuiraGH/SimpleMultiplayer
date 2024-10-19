using Core.CustomAnimationSystem;
using UnityEngine;

namespace Core
{
    public class CharacterIdleAnimation : CustomAnimationBase
    {
        public override float AnimationSpeed { get; protected set; } = 1f;
        public override string AnimationName { get; protected set; } = "Idle";
        protected override string AnimationAdditionalTag { get; set; } = string.Empty;
    }
}
