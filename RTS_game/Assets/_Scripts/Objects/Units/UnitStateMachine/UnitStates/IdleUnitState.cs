using RTS;
using RTS.Objects.Units;
using UnityEngine;

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
        SearchForTarget();
    }

    void SearchForTarget()
    {
        var hits = Physics.OverlapSphere(unit.transform.position, unit.unitStats.baseStats.aggroRange);
        foreach (var hit in hits)
        {
            var potentialTarget = hit.GetComponent<IAttackable>();
            if (potentialTarget != null && potentialTarget.GetTeam() != unit.GetTeam() && potentialTarget.GetTeam() != "Neutral" && potentialTarget.IsDead() == false)
            {
                unit.target = potentialTarget.GetTransform().gameObject;
                stateMachine.ChangeState(stateMachine.attackState);
                break;
            }

        }

    }
}
