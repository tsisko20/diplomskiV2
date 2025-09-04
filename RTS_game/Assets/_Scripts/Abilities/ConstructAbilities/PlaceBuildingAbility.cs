using RTS.Ability;
using RTS.Objects;
using RTS.Objects.Buildings;
using RTS.Objects.Units;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Place Building Ability")]
public class PlaceBuildingAbility : AbilityBase
{
    [SerializeField] private GameObject building;
    private float goldCost;
    private float woodCost;
    private Unit unit;
    TeamResourceStorages teamResourceStorages;

    public override void Setup(SelectableObject selectedObject)
    {
        if (selectedObject is Unit)
        {
            unit = (Unit)selectedObject;
        }
        if (building.layer == 8)
        {
            goldCost = building.GetComponent<Building>().GetBaseStats().baseStats.goldCost;
            woodCost = building.GetComponent<Building>().GetBaseStats().baseStats.woodCost;
        }

        switch (unit.tag)
        {
            case "Player":
                teamResourceStorages = ResourceHandler.instance.playerResStorage;
                break;
            case "Enemy":
                teamResourceStorages = ResourceHandler.instance.enemyResStorage;
                break;
        }
    }

    public override void Activate()
    {

        if (teamResourceStorages.GetGoldCount() >= goldCost && teamResourceStorages.GetWoodCount() >= woodCost)
        {
            if (unit.tag == "Player")
                BuildingConstructor.EnterConstructionMode(building);
            if (unit.tag == "Enemy")
            {
                EnemyContext enemyContext = GameObject.Find("GameController").GetComponent<EnemyContext>();
                Building buildingComponent;
                var clone = Instantiate(building, enemyContext.buildLocation.position, enemyContext.buildLocation.rotation);
                buildingComponent = clone.GetComponent<Building>();
                buildingComponent.state = BuildingState.Init;
                buildingComponent.SetTeam("Enemy");
                teamResourceStorages.UpdateResCount(ResourceType.Gold, (int)-goldCost);
                teamResourceStorages.UpdateResCount(ResourceType.Wood, (int)-woodCost);
            }
        }
    }

    public float GetWoodCost() => woodCost;
    public float GetGoldCost() => goldCost;

    public override void Deactivate()
    {
    }


}
