using RTS.Objects.Units;

public class IdleUnitState : UnitState
{
    public IdleUnitState(Unit _unit, UnitStateMachine _unitStateMachine) : base(_unit, _unitStateMachine)
    {
    }

    public override void EnterState()
    {
        unit.animator.ChangeAnimation(UnitStates.Idle);
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
