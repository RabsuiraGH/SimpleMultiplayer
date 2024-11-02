using Core.CustomAnimationSystem;
using UnityEngine;

namespace Core
{
    public class CharacterDeathAnimation : CustomAnimationBase
    {
        public override float AnimationSpeed { get; protected set; } = 1f;
        public override string AnimationName { get; protected set; } = "Death";
        protected override string AnimationAdditionalTag { get; set; } = string.Empty;

        public const string DEATH_TYPE = "Spin";
    }
}