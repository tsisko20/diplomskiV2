using RTS;
using RTS.Ability;
using RTS.Buildings;
using RTS.Objects;
using RTS.Objects.Buildings;
using RTS.Objects.Units;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Place Building Ability")]
public class PlaceBuildingAbility : AbilityBase
{
    [SerializeField] private GameObject building;
    private float buildingCost;
    private Unit unit;
    TeamResourceStorages teamResourceStorages;

    public override void Setup(SelectableObject selectedObject)
    {
        if (selectedObject is Unit)
        {
            unit = (Unit)selectedObject;
        }
        buildingCost = building.GetComponent<Building>().GetBaseStats().baseStats.cost;
    }

    public override void Activate()
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
        if (teamResourceStorages.GetGoldCount() >= buildingCost)
        {
            teamResourceStorages.UpdateResCount(ResourceType.Gold, (int)-buildingCost);
            unit.buildingConstructor.EnterConstructionMode(building);
        }
    }

    public override void Deactivate()
    {
    }


}
