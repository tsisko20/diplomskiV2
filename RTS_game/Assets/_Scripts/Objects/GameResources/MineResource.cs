using UnityEngine;

public class MineResource : MonoBehaviour, IGatherable
{
    public ResourceType GetResourceType()
    {
        return ResourceType.Gold;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public int GiveResource()
    {
        return 10;
    }
}
