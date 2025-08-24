using UnityEngine;

public class MineResource : MonoBehaviour, IGatherable
{
    public int health;

    private void Start()
    {
        health = 10000;
    }
    public ResourceType GetResourceType()
    {
        return ResourceType.Gold;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public int GiveResource(int amount)
    {
        int healthBeforeGather = health;
        health = health - amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            return healthBeforeGather;
        }
        return healthBeforeGather - health;
    }
}
