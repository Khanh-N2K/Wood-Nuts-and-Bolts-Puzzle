using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame_CapybaraRunner
{
    public class CapybaraState_GoOneBlock : StateMachineBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private string walkTrigger;

        private float speed;
        private Vector3 startPos;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.speed = animator.GetComponent<Capybara>().GetSpeed;
            this.startPos = animator.transform.position;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.transform.position = new(animator.transform.position.x + speed * Time.deltaTime,
                                                animator.transform.position.y,
                                                animator.transform.position.z);
            if (animator.transform.position.x - this.startPos.x >= 1)
                animator.SetTrigger(this.walkTrigger);
        }
    }
}
