using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private const string IS_DEAD = "IsDead";
    private const string IS_ATTACKING = "IsAttacking";
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayDieAnimation()
    {
        animator.SetTrigger(IS_DEAD);
    }
    public void PlayWalkingAnimation(bool state)
    {
        animator.SetBool(IS_WALKING, state);
    }
    public void PlayAttackAnimation(bool state)
    {
        animator.SetBool(IS_ATTACKING, state);
    }
}
