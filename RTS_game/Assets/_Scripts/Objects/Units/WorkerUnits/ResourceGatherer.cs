using System.Collections.Generic;
using UnityEngine;

public class ResourceGatherer : MonoBehaviour
{
    public static ResourceGatherer instance { get; private set; }
    private List<GameObject> allResourceObjects;
    private void Awake()
    {
        instance = this;
    }


    public static GameObject FindNearestResource(ResourceType resourceType, ref Vector3 previousTargetLocation)
    {
        float closestDistance = 0;
        GameObject newResource = null;
        closestDistance = float.MaxValue;
        switch (resourceType)
        {
            case ResourceType.Wood:
                instance.allResourceObjects = ResourceHandler.instance.allTreeObjects;
                break;
            case ResourceType.Gold:
                instance.allResourceObjects = ResourceHandler.instance.allGoldObjects;
                break;
        }
        foreach (var resource in instance.allResourceObjects)
        {
            if (resource == null)
                continue;
            float distance = Vector3.Distance(previousTargetLocation, resource.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                newResource = resource;
            }
        }
        if (newResource != null)
        {
            previousTargetLocation = newResource.transform.position;
        }
        return newResource;

    }
}