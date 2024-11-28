using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultersAnimator : BasicAnimator
{
    public bool isIdle = true;

    public Animator assaulter1Animator;
    public Animator assaulter2Animator;
    public Animator weapon1Animator;
    public Animator weapon2Animator;

    [SerializeField] public float changeAnimationMinTime;
    [SerializeField] public float changeAnimationMaxTime;

    public float leftTimeForChangeAssaulter1Animation;
    public float leftTimeForChangeAssaulter2Animation;

    public string[] triggers = { "reload", "look_around" };


    public override void Start()
    {
        ResetChangeAnimationTimerForAssaulter1();
        ResetChangeAnimationTimerForAssaulter2();
    }


    void Update()
    {
        UpdateIsIdleState();
        if (isIdle)
        {
            UpdateChangeAnimationTimer();
        }
    }

    public void UpdateIsIdleState()
    {
        AnimatorStateInfo stateInfo1 = assaulter1Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo stateInfo2 = assaulter2Animator.GetCurrentAnimatorStateInfo(0);
        
        bool isAssaulter1Idle = stateInfo1.IsName("idle");
        bool isAssaulter2Idle = stateInfo2.IsName("idle");

        isIdle = isAssaulter1Idle && isAssaulter2Idle;
    }

    private void UpdateChangeAnimationTimer()
    {
        leftTimeForChangeAssaulter1Animation -= Time.deltaTime;
        leftTimeForChangeAssaulter2Animation -= Time.deltaTime;

        if (leftTimeForChangeAssaulter1Animation <= 0)
        {
            string randomTrigger = triggers[Random.Range(0, triggers.Length)];

            assaulter1Animator.SetTrigger(randomTrigger);
            weapon1Animator.SetTrigger(randomTrigger);

            ResetChangeAnimationTimerForAssaulter1();
        }

        if (leftTimeForChangeAssaulter2Animation <= 0)
        {
            string randomTrigger = triggers[Random.Range(0, triggers.Length)];

            assaulter2Animator.SetTrigger(randomTrigger);
            weapon2Animator.SetTrigger(randomTrigger);

            ResetChangeAnimationTimerForAssaulter2();
        }
    }

    public void ResetChangeAnimationTimerForAssaulter1()
    {
        float randomTime = Random.Range(changeAnimationMinTime, changeAnimationMaxTime);
        leftTimeForChangeAssaulter1Animation = randomTime;
    }

    public void ResetChangeAnimationTimerForAssaulter2()
    {
        float randomTime = Random.Range(changeAnimationMinTime, changeAnimationMaxTime);
        leftTimeForChangeAssaulter2Animation = randomTime;
    }

    public override void Attack()
    {
        assaulter1Animator.SetTrigger("attack");
        weapon1Animator.SetTrigger("attack");

        assaulter2Animator.SetTrigger("attack");
        weapon2Animator.SetTrigger("attack");
    }

    public override void Death()
    {
        assaulter1Animator.SetTrigger("death");
        weapon1Animator.SetTrigger("death");

        assaulter2Animator.SetTrigger("death");
        weapon2Animator.SetTrigger("death");
    }

    public override void StartMove()
    {
        assaulter1Animator.SetBool("moving", true);
        weapon1Animator.SetBool("moving", true);

        assaulter2Animator.SetBool("moving", true);
        weapon2Animator.SetBool("moving", true);
    }

    public override void FinishMove()
    {
        assaulter1Animator.SetBool("moving", false);
        weapon1Animator.SetBool("moving", false);

        assaulter2Animator.SetBool("moving", false);
        weapon2Animator.SetBool("moving", false);
    }
}
