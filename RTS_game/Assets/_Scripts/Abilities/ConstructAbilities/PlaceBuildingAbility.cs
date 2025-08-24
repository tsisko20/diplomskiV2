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
        if (building.layer == 8)
            buildingCost = building.GetComponent<Building>().GetBaseStats().baseStats.goldCost;

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

        if (teamResourceStorages.GetGoldCount() >= buildingCost)
        {
            unit.buildingConstructor.EnterConstructionMode(building);
        }
    }

    public override void Deactivate()
    {
    }


}
