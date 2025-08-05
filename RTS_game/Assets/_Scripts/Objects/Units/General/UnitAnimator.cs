using RTS.Objects.Units;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private const string IS_DEAD = "IsDead";
    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_GATHERING = "IsGathering";
    UnitState currentState;
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

    public void ChangeAnimation(UnitState state)
    {
        ChangeBoolValueInAnimator(currentState, false);
        currentState = state;
        ChangeBoolValueInAnimator(state, true);
    }

    private void ChangeBoolValueInAnimator(UnitState state, bool value)
    {
        switch (state)
        {
            case UnitState.Walking:
                PlayWalkingAnimation(value); break;
            case UnitState.Attacking:
                PlayAttackAnimation(value); break;
            case UnitState.Gathering:
                PlayGatheringAnimation(value); break;
            case UnitState.Dead:
                PlayDieAnimation(); break;
            case UnitState.Idle: break;
        }
    }
}
