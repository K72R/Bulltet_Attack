using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 direction)
    {
        if (animator == null) return;

        if (direction != Vector2.zero)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    public void Shoot()
    {
        if (animator == null) return;

        animator.SetTrigger("Attack");
    }

    public void Reload()
    {
        if (animator == null) return;

        animator.SetTrigger("Reload");
    }

    public void RefreshAnimator(Animator newAnimator)
    {
        animator = newAnimator;
    }
}
