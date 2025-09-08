using RTS.Objects.Units;
using System.Collections.Generic;
using UnityEngine;

public class GatherUnitState : UnitState
{
    IGatherable targetResource;
    public Vector3 previousTargetLocation = Vector3.zero;
    public ResourceType resourceType;
    private float gatheringSpeed = 1;
    public float gatheringTimer;
    const float TIME_TO_GATHER = 1f;
    public int goldCollected = 0;
    private int maxGold = 20;
    public int woodCollected = 0;
    private int maxWood = 20;
    private int gatheringPower = 5;
    private Transform closestResourceStorage;
    private Transform closestResource;
    private TeamResourceStorages teamResourceStorages;
    public GatherUnitState(Unit _unit, UnitStateMachine _stateMachine) : base(_unit, _stateMachine)
    {
    }

    public override void EnterState()
    {
        switch (unit.tag)
        {
            case "Player":
                teamResourceStorages = ResourceHandler.instance.playerResStorage;
                break;
            case "Enemy":
                teamResourceStorages = ResourceHandler.instance.enemyResStorage;
                break;
        }
        targetResource = unit.target.GetComponent<IGatherable>();
        resourceType = targetResource.GetResourceType();
        closestResource = targetResource.GetTransform();
        previousTargetLocation = closestResource.position;
    }

    public override void Update()
    {
        if (unit.target == null && targetResource != null && previousTargetLocation == Vector3.zero)
        {
            targetResource = null;
            gatheringTimer = TIME_TO_GATHER;
        }

        if ((MonoBehaviour)targetResource != null)
        {
            if ((resourceType == ResourceType.Gold && goldCollected == maxGold) || (resourceType == ResourceType.Wood && woodCollected == maxWood))
            {
                if (teamResourceStorages.allResourceStorages.Count == 0)
                {
                    stateMachine.ChangeState(stateMachine.idleState);
                    return;
                }
                GoToResourceStorage();
            }
            else
            {
                GoToResource();
            }
        }
        else
            FindNewResource();
    }

    private void FindNewResource()
    {
        if (previousTargetLocation != Vector3.zero)
        {
            FindNearestResource();
            if (closestResource == null)
            {
                stateMachine.ChangeState(stateMachine.idleState);
            }
        }
    }

    private void GoToResourceStorage()
    {
        FindNearestResourceStorage();
        if (closestResourceStorage != null)
            if (CalculateTargetDistance(closestResourceStorage.gameObject) <= 1.2f)
            {
                GiveResources();
                unit.StopMoving();
            }
            else
            {
                MoveToResStorage();
            }
    }
    private void GoToResource()
    {
        if (CalculateTargetDistance(targetResource.GetTransform().gameObject) <= 1f)
        {
            GatherResource();
            unit.StopMoving();
        }
        else
        {
            MoveToResource();
        }
    }
    public void SetResourceTarget(IGatherable gatherable)
    {
        targetResource = gatherable;
        switch (targetResource.GetResourceType())
        {
            case ResourceType.Gold:
                resourceType = ResourceType.Gold; break;
            case ResourceType.Wood:
                resourceType = ResourceType.Wood; break;
        }
        closestResource = targetResource.GetTransform();
        previousTargetLocation = closestResource.position;
    }

    private void IncreaseResourceCollected()
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                if (goldCollected < maxGold)
                {
                    goldCollected += targetResource.GiveResource(gatheringPower);
                    goldCollected = Mathf.Clamp(goldCollected, 0, maxGold);
                }
                break;
            case ResourceType.Wood:
                if (woodCollected < maxWood)
                {
                    woodCollected += targetResource.GiveResource(gatheringPower);
                    woodCollected = Mathf.Clamp(woodCollected, 0, maxWood);
                }
                break;
        }
    }

    private void FindNearestResourceStorage()
    {
        float closestDistance = 0;

        closestDistance = float.MaxValue;
        foreach (var resourceStorage in teamResourceStorages.allResourceStorages)
        {
            if (resourceStorage == null)
                continue;
            float distance = Vector3.Distance(unit.transform.position, resourceStorage.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestResourceStorage = resourceStorage.transform;
            }
        }
    }

    private void FindNearestResource()
    {
        GameObject newResource = null;
        newResource = ResourceGatherer.FindNearestResource(resourceType, ref previousTargetLocation);
        unit.target = newResource;
        closestResource = newResource.transform;
        targetResource = newResource.GetComponent<IGatherable>();
    }
    private void MoveToResStorage()
    {
        if (teamResourceStorages.allResourceStorages.Count == 0)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
        Collider targetCollider = closestResourceStorage.GetComponent<Collider>();

        Vector3 targetCenter = closestResourceStorage.transform.position;

        Vector3 closestPoint = targetCollider != null
            ? targetCollider.ClosestPoint(unit.transform.position)
            : targetCenter;
        unit.MoveTo(closestPoint);

    }

    private void MoveToResource()
    {
        Collider targetCollider = closestResource.GetComponent<Collider>();

        Vector3 targetCenter = closestResource.transform.position;

        Vector3 closestPoint = targetCollider != null
            ? targetCollider.ClosestPoint(unit.transform.position)
            : targetCenter;
        unit.MoveTo(closestPoint);
    }

    private void GatherResource()
    {
        unit.animator.ChangeAnimation(UnitStates.Gathering);
        if (gatheringTimer < 0f)
        {
            IncreaseResourceCollected();
            gatheringTimer = TIME_TO_GATHER;
        }
        else
        {
            gatheringTimer -= Time.deltaTime * gatheringSpeed;
        }
    }
    private void GiveResources()
    {
        teamResourceStorages.UpdateResCount(ResourceType.Gold, goldCollected);
        teamResourceStorages.UpdateResCount(ResourceType.Wood, woodCollected);
        goldCollected = 0;
        woodCollected = 0;
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

}