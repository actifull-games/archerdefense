using UnityEngine;

namespace Animation
{
    public class DieAnimationBehaviour : StateMachineBehaviour
    {
        private static readonly int Exit = Animator.StringToHash("Exit");
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            animator.SetBool(Exit, true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.StopPlayback();
            animator.enabled = false;
        }
    }
}
