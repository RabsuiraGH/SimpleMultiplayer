using UnityEngine;

namespace Core
{
    public class EmptyState : StateMachineBehaviour
    {
        private CharacterManager _character;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_character == null)
                _character = animator.GetComponent<CharacterManager>();

            _character.IsPerformingMainAction = false;
            if (_character.CharacterMovementManager.IsJumping) _character.CharacterMovementManager.StopJumping();
            if (_character.IsOwner) // TODO: REDURANT?
            {
                _character.CharacterAttackManager.StopAttackState();
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_character.IsOwner)
            {
                _character.CharacterAttackManager.StopAttackState();
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}