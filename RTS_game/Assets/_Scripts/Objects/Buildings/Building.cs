using RTS;
using RTS.Buildings;
using RTS.InputManager;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class Building : SelectableObject, IAttackable
{
    [SerializeField] private BasicBuilding buildingStats;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = buildingStats.baseStats.health;
        SetTeam();
        SetMaterial();
    }



    // Update is called once per frame
    void Update()
    {
    }

    public override string GetObjectName()
    {
        return buildingStats.buildingName;
    }

    public override BasicObject GetBaseStats()
    {
        return buildingStats;
    }


    public Transform GetTransform() => transform;

    public override bool IsDead() => currentHealth <= 0;

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            Die();
        }
    }
    public void Die()
    {
        RTS.InputManager.InputHandler.instance.selectedObjects.Remove(gameObject.GetComponent<Transform>());
        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;
    }

}
