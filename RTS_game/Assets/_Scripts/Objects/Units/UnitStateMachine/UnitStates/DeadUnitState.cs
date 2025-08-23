using RTS.Objects.Units;
using UnityEngine;

public class DeadUnitState : UnitState
{
    public DeadUnitState(Unit _unit, UnitStateMachine _stateMachine) : base(_unit, _stateMachine)
    {
    }

    public override void EnterState()
    {
        RTS.InputManager.InputHandler.instance.GetSelectableObjects().Remove(unit.gameObject.GetComponent<Transform>());
        unit.navAgent.isStopped = true;
        unit.navAgent.ResetPath();
        Collider col = unit.GetComponent<Collider>();
        if (col) col.enabled = false;
        Transform movingRange = unit.transform.Find("MovingRange");
        movingRange.gameObject.SetActive(false);
        unit.animator.ChangeAnimation(UnitStates.Dead);
        unit.Deselect();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        base.Update();
    }
}
