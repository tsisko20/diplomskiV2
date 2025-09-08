using RTS.Objects.Buildings;
using RTS.Objects.Units;
using UnityEngine;

public class ConstructUnitState : UnitState
{
    Building targetBuilding;
    public float repairTimer;
    public float repairSpeed = 1f;
    const float TIME_TO_REPAIR = 1f;
    float targetMaxHealth;
    public ConstructUnitState(Unit _unit, UnitStateMachine _stateMachine) : base(_unit, _stateMachine)
    {
    }

    public override void EnterState()
    {
        targetBuilding = unit.target.GetComponent<Building>();
        targetMaxHealth = targetBuilding.GetBaseStats().baseStats.health;
        repairTimer = TIME_TO_REPAIR;
    }
    public override void Update()
    {
        if (targetBuilding == null)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
        if (targetBuilding.GetCurrentHealth() == targetMaxHealth)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
        if (CalculateTargetDistance(unit.target) <= 1)
        {
            unit.StopMoving();
            if (repairTimer <= 0)
            {
                RepairBuilding();
                repairTimer = TIME_TO_REPAIR;
            }
            else
                unit.animator.ChangeAnimation(UnitStates.Repairing);
            repairTimer -= Time.deltaTime * repairSpeed;
        }
        else
        {
            unit.MoveTo(unit.target.transform.position);
        }
    }

    private float CalculateTargetDistance(GameObject destinationObject)
    {
        Collider targetCollider = destinationObject.GetComponent<Collider>();

        Vector3 targetCenter = destinationObject.transform.position;

        Vector3 closestPoint = targetCollider != null
            ? targetCollider.ClosestPoint(unit.transform.position)
            : targetCenter;

        float distanceToCollider = Vector3.Distance(unit.transform.position, closestPoint);
        return distanceToCollider;
    }

    private void RepairBuilding()
    {
        targetBuilding.IncreaseCurrentHealth(20);
    }
}
