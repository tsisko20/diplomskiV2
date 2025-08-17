using UnityEngine;

public class WoodResource : MonoBehaviour, IGatherable
{
    public int health;

    private void Start()
    {
        health = 100;
    }

    public ResourceType GetResourceType() => ResourceType.Wood;

    public Transform GetTransform() => transform;

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
