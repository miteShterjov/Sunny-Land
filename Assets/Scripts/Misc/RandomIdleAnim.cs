using UnityEngine;

namespace Misc
{
    [RequireComponent(typeof(Animator))]
    public class RandomIdleAnim : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (!animator) return;

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(stateInfo.fullPathHash, -1, Random.Range(0f, 1f));
        }
    }
}

