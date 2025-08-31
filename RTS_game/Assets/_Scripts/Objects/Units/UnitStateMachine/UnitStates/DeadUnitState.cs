using RTS.Objects.Units;
using UnityEngine;

public class DeadUnitState : UnitState
{
    private float deathTime;
    public DeadUnitState(Unit _unit, UnitStateMachine _stateMachine) : base(_unit, _stateMachine)
    {
    }

    public override void EnterState()
    {
        RTS.InputManager.InputHandler.GetSelectableObjects().Remove(unit.gameObject.GetComponent<Transform>());
        unit.navAgent.isStopped = true;
        unit.navAgent.ResetPath();
        unit.navAgent.enabled = false;
        Transform movingRange = unit.transform.Find("MovingRange");
        movingRange.gameObject.SetActive(false);
        unit.animator.ChangeAnimation(UnitStates.Dead);
        unit.Deselect();
        deathTime = Time.time;
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {
        if (Time.time - deathTime >= 5f)
        {
            GameObject.Destroy(unit.gameObject);
        }
    }
}
