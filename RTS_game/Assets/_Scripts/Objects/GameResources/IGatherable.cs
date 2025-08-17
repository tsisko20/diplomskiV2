using UnityEngine;

public interface IGatherable : ITargetable
{
    public int GiveResource(int amount);
    public ResourceType GetResourceType();
}
