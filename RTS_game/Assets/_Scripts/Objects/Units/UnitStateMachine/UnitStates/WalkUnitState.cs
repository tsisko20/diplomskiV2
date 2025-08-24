using RTS.Objects.Units;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
                Debug.Log("stop1");
                stateMachine.ChangeState(stateMachine.idleState);
            }
            else
            {
                foreach (Unit unitInMovingRange in unit.movingRange.GetUnitsInMovingRange())
                {

                    if (unitInMovingRange.GetCurrentDestination() == unit.GetCurrentDestination() && unitInMovingRange.animator.currentState != UnitStates.Walking && unit.target == null)
                    {
                        stateMachine.ChangeState(stateMachine.idleState);
                        Debug.Log("stop2");
                    }
                }
            }
        }
    }
}