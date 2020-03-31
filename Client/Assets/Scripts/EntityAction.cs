using System.Collections;
using UnityEngine;

public class EntityAction : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float woodcuttingAnimationLength = 2f;

    public void Woodcut()
    {
        if (!animator) return;
        animator.SetBool("Woodcutting", true);
        animator.SetInteger("ActionNumber", 0);
        animator.SetTrigger("Action");

        StartCoroutine(EndWoodcutting());
    }

    private IEnumerator EndWoodcutting()
    {
        if (!animator) yield break;
        yield return new WaitForSeconds(woodcuttingAnimationLength);
        animator.SetBool("Woodcutting", false);
    }
}
