using UnityEngine;


public class AnimToggler : MonoBehaviour
{
    public Animator[] animators;
    public Transform content;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        animators = content.GetComponentsInChildren<Animator>();
    }

    public void Attack()
    {
        foreach (var animator in animators)
        {
            animator.SetTrigger("attack");
        }
    }

    public void Death()
    {
        foreach (var animator in animators)
        {
            animator.SetTrigger("death");
        }
    }

    public void AttackAndDeath()
    {
        Attack();
        Death();
    }
}
