using Mono.Cecil;
using RTS;
using RTS.Objects.Units;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGatherer : MonoBehaviour
{
    IGatherable targetResource;
    public Vector3 previousTargetLocation = Vector3.zero;
    public ResourceType resourceType;
    private Unit unit;
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

    private void Awake()
    {
        unit = GetComponent<Unit>();
        gatheringTimer = TIME_TO_GATHER;
    }

    private void Start()
    {
        switch (unit.GetTeam())
        {
            case Team.Player:
                teamResourceStorages = ResourceHandler.instance.playerResStorage;
                break;
            case Team.Enemy:
                teamResourceStorages = ResourceHandler.instance.enemyResStorage;
                break;
        }
    }

    private void Update()
    {
        if ((MonoBehaviour)unit.target == null && targetResource != null && previousTargetLocation == Vector3.zero)
        {
            Debug.Log("brisi target");
            targetResource = null;
            gatheringTimer = TIME_TO_GATHER;
        }



        if (((MonoBehaviour)targetResource) != null)
        {
            Debug.Log((MonoBehaviour)targetResource);
            if ((resourceType == ResourceType.Gold && goldCollected == maxGold) || (resourceType == ResourceType.Wood && woodCollected == maxWood))
            {
                if (teamResourceStorages.allResourceStorages.Count == 0)
                {
                    unit.ChangeState(UnitStates.Idle);
                    return;
                }
                FindNearestResourceStorage();
                MoveToResStorage();
            }
            else
            {
                MoveToResource();
            }
            if (unit.state == UnitStates.Gathering)
            {
                GatherResource();
            }
        }
        else if (previousTargetLocation != Vector3.zero)
        {
            FindNearestResource();
            if (closestResource == null)
                unit.ChangeState(UnitStates.Idle);
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
            float distance = Vector3.Distance(transform.position, resourceStorage.transform.position);
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
            previousTargetLocation = newResource.transform.position;
            Debug.Log("pronaden sljedeci resurs");
        }
    }
    private void MoveToResStorage()
    {
        if (teamResourceStorages.allResourceStorages.Count == 0)
        {
            unit.ChangeState(UnitStates.Idle);
            return;
        }
        unit.MoveToTarget(closestResourceStorage, GiveResources);

    }

    private void MoveToResource()
    {
        if (targetResource != null && unit.target != null)
            unit.MoveToTarget(closestResource, () => { unit.ChangeState(UnitStates.Gathering); });
    }

    private void GatherResource()
    {
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
}