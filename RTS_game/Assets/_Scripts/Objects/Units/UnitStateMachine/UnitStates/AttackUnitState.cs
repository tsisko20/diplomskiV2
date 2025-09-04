using RTS;
using RTS.Objects.Units;
using UnityEngine;

public class AttackUnitState : UnitState
{
    [SerializeField] private IAttackable attackTarget;
    private float attackCooldown;
    public AttackUnitState(Unit _unit, UnitStateMachine _unitStateMachine) : base(_unit, _unitStateMachine)
    {
    }

    public override void EnterState()
    {
        attackTarget = unit.target.GetComponent<IAttackable>();
    }

    public override void ExitState()
    {
        attackTarget = null;
    }

    public override void Update()
    {
        if (unit.IsDead())
        {
            return;
        }
        if (unit.target == null || attackTarget.IsDead())
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }

        Collider targetCollider = attackTarget.GetTransform().GetComponent<Collider>();
        Vector3 closestPoint = targetCollider.ClosestPoint(unit.transform.position);
        float distance = Vector3.Distance(unit.transform.position, closestPoint);

        if (distance <= unit.unitStats.baseStats.attackRange)
        {
            unit.StopMoving();
            AttackTarget();
        }
        else
        {
            unit.MoveTo(attackTarget.GetTransform().position);
        }
    }

    private void AttackTarget()
    {
        unit.animator.ChangeAnimation(UnitStates.Attacking);
        Vector3 directionToTarget = attackTarget.GetTransform().position - unit.transform.position;
        directionToTarget.y = 0f;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0)
        {
            attackTarget.TakeDamage(unit.unitStats.baseStats.attackDamage);
            attackCooldown = unit.unitStats.baseStats.attackSpeed;
        }
    }
}
