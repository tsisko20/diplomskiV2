using RTS.Objects.Units;
using UnityEngine;


public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private const string IS_DEAD = "IsDead";
    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_GATHERING = "IsGathering";
    UnitStates currentState;
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
        ChangeBoolValueInAnimator(currentState, false);
        currentState = state;
        ChangeBoolValueInAnimator(state, true);
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
                PlayGatheringAnimation(value); break;
            case UnitStates.Dead:
                PlayDieAnimation(); break;
            case UnitStates.Idle: break;
        }
    }
}
