using UnityEngine;

public interface IGatherable : ITargetable
{
    public int GiveResource();
    public ResourceType GetResourceType();
}
