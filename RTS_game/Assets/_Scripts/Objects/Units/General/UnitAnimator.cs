using RTS.Objects.Units;
using UnityEngine;


public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private const string IS_DEAD = "IsDead";
    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_GATHERING = "IsGathering";
    public UnitStates currentState;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void PlayDieAnimation()
    {
        animator.SetTrigger(IS_DEAD);
    }
    private void PlayWalkingAnimation(bool state)
    {
        animator.SetBool(IS_WALKING, state);
    }
    private void PlayAttackAnimation(bool state)
    {
        animator.SetBool(IS_ATTACKING, state);
    }

    private void PlayGatheringAnimation(bool state)
    {
        animator.SetBool(IS_GATHERING, state);
    }

    public void ChangeAnimation(UnitStates state)
    {
        if (currentState == state)
            return;
        ChangeBoolValueInAnimator(currentState, false);
        ChangeBoolValueInAnimator(state, true);
        currentState = state;
    }

    private void ChangeBoolValueInAnimator(UnitStates state, bool value)
    {
        switch (state)
        {
            case UnitStates.Walking:
                PlayWalkingAnimation(value); break;
            case UnitStates.Attacking:
                PlayAttackAnimation(value); break;
            case UnitStates.Gathering:
            case UnitStates.Repairing:
                PlayGatheringAnimation(value);
                break;
            case UnitStates.Dead:
                PlayDieAnimation(); break;
        }
    }
}
