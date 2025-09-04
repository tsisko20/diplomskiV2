using RTS.Objects.Units;

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
        unit.animator.ChangeAnimation(UnitStates.Idle);
        unit.navAgent.isStopped = true;
    }

    public override void Update()
    {
        if (!unit.navAgent.pathPending && unit.navAgent.hasPath)
        {

            if (unit.navAgent.remainingDistance <= unit.navAgent.stoppingDistance)
            {
                stateMachine.ChangeState(stateMachine.idleState);
            }
            else
            {
                foreach (Unit unitInMovingRange in unit.movingRange.GetUnitsInMovingRange())
                {
                    if (unitInMovingRange.IsDead() == false)
                        if (unitInMovingRange.GetCurrentDestination() == unit.GetCurrentDestination() && unitInMovingRange.animator.currentState != UnitStates.Walking && unit.target == null)
                        {
                            stateMachine.ChangeState(stateMachine.idleState);
                        }
                }
            }
        }
    }
}