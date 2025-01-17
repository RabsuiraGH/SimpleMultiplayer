using System.Linq;
using UnityEngine;

namespace Core.CustomAnimationSystem
{
    [RequireComponent(typeof(Animator))]
    public class CustomAnimator : MonoBehaviour, ICustomAnimator
    {
        [SerializeField] protected string _currentAnimationState = string.Empty;

        [SerializeField] protected string _animatorTag = string.Empty;

        [SerializeField] protected bool _isAnimationPlaying = false;

        [SerializeField] protected Animator _animator = null;

        protected virtual void Awake()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }

            if (_animatorTag == string.Empty)
            {
                if (transform.parent != null)
                    _animatorTag = transform.parent.name;
                else
                    _animatorTag = transform.name;
            }
        }

        public virtual bool IsAnimationPlaying()
        {
            return _isAnimationPlaying;
        }

        public virtual void PlayAnimation(CustomAnimationBase animationAction)
        {
            string stringState = GetStateString(animationAction);

            ChangeAnimationState(stringState, animationAction.AnimationSpeed);
        }

        protected virtual void ChangeAnimationState(string newState, float animationSpeed)
        {
            if (_currentAnimationState == newState) return;

            _animator.speed = animationSpeed;

            if (!_animator.HasState(0, Animator.StringToHash(newState)))
                Debug.LogError(($"NO ANIMATION WITH NAME: {newState}"));

            _animator.Play(newState);

            _isAnimationPlaying = true;

            _currentAnimationState = newState;
        }

        public void SetFloat(string parameter, float value)
        {
            _animator.SetFloat(parameter, value);
        }

        public void SetBool(string parameter, bool value)
        {
            _animator.SetBool(parameter, value);
        }

        public void SetInt(string parameter, int value)
        {
            _animator.SetInteger(parameter, value);
        }

        protected virtual string GetStateString(CustomAnimationBase animation)
        {
            return _animatorTag + "_" + animation.AnimationName + animation.GetTag();
        }

        protected virtual string GetCutStateString(CustomAnimationBase animation)
        {
            return _animatorTag + "_" + animation.AnimationName;
        }

        public virtual float GetClipLengthInSeconds(CustomAnimationBase animation)
        {
            AnimationClip clip =
                _animator.runtimeAnimatorController.animationClips.FirstOrDefault(
                    x => x.name == GetStateString(animation));
            if (clip != null) return clip.length;


            clip = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(
                x => x.name.StartsWith(GetStateString(animation)));

            if (clip != null) return clip.length;

            clip = _animator.runtimeAnimatorController.animationClips.FirstOrDefault(
                x => x.name.StartsWith(GetCutStateString(animation)));

            if (clip != null) return clip.length;

            return -1;
        }
    }
}