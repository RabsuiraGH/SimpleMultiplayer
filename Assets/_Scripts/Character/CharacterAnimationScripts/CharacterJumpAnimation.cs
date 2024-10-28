using Core.CustomAnimationSystem;
using UnityEngine;

namespace Core
{
    public class CharacterJumpAnimation : CustomAnimationBase
    {
        public override float AnimationSpeed { get; protected set; } = 1f;
        public override string AnimationName { get; protected set; } = "Jump";
        protected override string AnimationAdditionalTag { get; set; } = string.Empty;
    }
}