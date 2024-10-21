using Core.CustomAnimationSystem;

namespace Core
{
    public class CharacterWalkAnimation : CustomAnimationBase
    {
        public override float AnimationSpeed { get; protected set; } = 1f;
        public override string AnimationName { get; protected set; } = "Walk";
        protected override string AnimationAdditionalTag { get; set; } = string.Empty;
    }
}