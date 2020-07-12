using System.Linq;
using UnityEngine;

public class EntityAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController defaultController;

    void Start()
    {
        if (!animator) animator = gameObject.GetComponentInChildren<Animator>();
        if (animator && defaultController && !animator.runtimeAnimatorController)
        {
            animator.runtimeAnimatorController = defaultController;
        }
    }

    public void SetAnimationState(string animationState, bool enabled, bool trigger, int action)
    {
        if (!animator)
        {
            Debug.LogError("No animator set for entity: " + this.name);
            return;
        }

        animator.SetBool(animationState, enabled);
        animator.SetInteger("ActionNumber", action);
        animator.SetBool("Action", trigger);
    }
}
