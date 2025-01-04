using UnityEngine;

namespace Minigame_CapybaraRunner
{
    public class CapybaraState_Walking : StateMachineBehaviour
    {
        [Header("Paramters")] 
        [SerializeField] private KeyCode eatKey;
        [SerializeField] private string eatTrigger;

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float speed = animator.GetComponent<Capybara>().GetSpeed;
            animator.transform.position = new(animator.transform.position.x + speed * Time.deltaTime,
                                                animator.transform.position.y,
                                                animator.transform.position.z);

            this.CheckEat(animator);
        }

        private void CheckEat(Animator animator)
        {
            if (Input.GetKeyDown(this.eatKey))
            {
                animator.SetTrigger(this.eatTrigger);
            }
        }
    }
}
