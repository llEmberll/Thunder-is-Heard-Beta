using UnityEngine;

public class OneCharAnimator : BasicAnimator
{
    public bool isIdle = true;

    public Animator bodyAnimator;
    public Animator weaponAnimator;

    [SerializeField] public float changeAnimationMinTime;
    [SerializeField] public float changeAnimationMaxTime;

    public float leftTimeForChangeAnimation;

    public string[] triggers = { "reload", "look_around" };


    public override void Start()
    {
        ResetChangeAnimationTimer();
    }


    public virtual void Update()
    {
        UpdateIsIdleState();
        if (isIdle)
        {
            UpdateChangeAnimationTimer();
        }
    }

    public virtual void UpdateIsIdleState()
    {
        AnimatorStateInfo stateInfo = bodyAnimator.GetCurrentAnimatorStateInfo(0);
        isIdle = stateInfo.IsName("idle");
    }

    public virtual void UpdateChangeAnimationTimer()
    {
        leftTimeForChangeAnimation -= Time.deltaTime;

        if (leftTimeForChangeAnimation <= 0)
        {
            string randomTrigger = triggers[Random.Range(0, triggers.Length)];

            bodyAnimator.SetTrigger(randomTrigger);
            weaponAnimator.SetTrigger(randomTrigger);

            ResetChangeAnimationTimer();
        }
    }

    public virtual void ResetChangeAnimationTimer()
    {
        float randomTime = Random.Range(changeAnimationMinTime, changeAnimationMaxTime);
        leftTimeForChangeAnimation = randomTime;
    }

    public override void Attack()
    {
        bodyAnimator.SetTrigger("attack");
        weaponAnimator.SetTrigger("attack");
    }

    public override void Death()
    {
        bodyAnimator.SetTrigger("death");
        weaponAnimator.SetTrigger("death");
    }

    public override void StartMove()
    {
        bodyAnimator.SetBool("moving", true);
        weaponAnimator.SetBool("moving", true);
    }

    public override void FinishMove()
    {
        bodyAnimator.SetBool("moving", false);
        weaponAnimator.SetBool("moving", false);
    }
}
