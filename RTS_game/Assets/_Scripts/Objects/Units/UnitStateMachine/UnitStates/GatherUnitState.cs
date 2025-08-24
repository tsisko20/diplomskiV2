using RTS;
using RTS.Objects.Units;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    private List<GameObject> allResourceObjects;
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

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        if (unit.target == null && targetResource != null && previousTargetLocation == Vector3.zero)
        {
            Debug.Log("brisi target");
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
                FindNearestResourceStorage();
                if (CalculateTargetDistance(closestResourceStorage.gameObject) <= 1f)
                {
                    unit.StopMoving();
                    Debug.Log("give resource to storage");
                    GiveResources();
                }
                else
                {
                    Debug.Log("move to storage");
                    MoveToResStorage();
                }
            }
            else
            {
                if (CalculateTargetDistance(targetResource.GetTransform().gameObject) <= 1f)
                {
                    unit.StopMoving();
                    Debug.Log("gather");
                    GatherResource();
                }
                else
                {
                    Debug.Log("move to resource");
                    MoveToResource();
                }
            }
        }
        else if (previousTargetLocation != Vector3.zero)
        {
            FindNearestResource();
            if (closestResource == null)
                stateMachine.ChangeState(stateMachine.idleState);
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
                    Debug.Log("bum drvo");
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
        float closestDistance = 0;
        GameObject newResource = null;
        closestDistance = float.MaxValue;

        switch (resourceType)
        {
            case ResourceType.Wood:
                allResourceObjects = ResourceHandler.instance.allTreeObjects;
                break;
            case ResourceType.Gold:
                allResourceObjects = ResourceHandler.instance.allGoldObjects;
                break;
        }

        foreach (var resource in allResourceObjects)
        {
            if (resource == null)
                continue;
            float distance = Vector3.Distance(previousTargetLocation, resource.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestResource = resource.transform;
                newResource = resource;
                targetResource = resource.GetComponent<IGatherable>();
            }
        }
        if (newResource != null)
        {
            unit.target = newResource;
            previousTargetLocation = newResource.transform.position;
            Debug.Log("pronaden sljedeci resurs");
        }
    }
    private void MoveToResStorage()
    {
        if (teamResourceStorages.allResourceStorages.Count == 0)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
        unit.MoveTo(closestResourceStorage.transform.position);

    }

    private void MoveToResource()
    {
        unit.MoveTo(closestResource.position);
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