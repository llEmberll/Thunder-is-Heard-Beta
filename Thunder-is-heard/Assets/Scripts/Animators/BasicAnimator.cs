using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class BasicAnimator : MonoBehaviour, IAnimator
{
    public Animator[] animators;

    public virtual void Start()
    {
        InitAnimators();
    }

    public virtual void InitAnimators()
    {
        animators = GetComponentsInChildren<Animator>();
    }


    public virtual void Attack()
    {
        foreach (var animator in animators)
        {
            if (animator != null)
            {
                animator.SetTrigger("attack");
            }
        }
    }

    public virtual void Death()
    {
        foreach (var animator in animators)
        {
            if (animator != null)
            {
                animator.SetTrigger("death");
            }
        }
    }

    public virtual void StartMove()
    {
        foreach (var animator in animators)
        {
            if (animator != null)
            {
                animator.SetBool("moving", true);
            }
        }
    }

    public virtual void FinishMove()
    {
        foreach (var animator in animators)
        {
            if (animator != null)
            {
                animator.SetBool("moving", false);
            }
        }
    }
}
