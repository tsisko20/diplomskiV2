using RTS.Objects.Units;
using UnityEngine;

public class WalkUnitState : UnitState
{
    public WalkUnitState(Unit _unit, UnitStateMachine _unitStateMachine) : base(_unit, _unitStateMachine)
    {
    }

    public override void EnterState()
    {
        unit.navAgent.isStopped = false;
        unit.animator.ChangeAnimation(UnitStates.Walking);
        unit.navAgent.SetDestination(unit.destination);
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
