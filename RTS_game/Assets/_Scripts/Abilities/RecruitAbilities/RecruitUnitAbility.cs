using RTS.Ability;
using RTS.Objects;
using RTS.Objects.Buildings;
using RTS.Objects.Units;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Recruit Unit Ability")]
public class RecruitUnitAbility : AbilityBase
{
    [SerializeField] GameObject unit;
    private Building building;
    TeamResourceStorages teamResourceStorages;
    private float goldCost;
    private float woodCost;
    public override void Activate()
    {
        if (unit.layer == 7 && (teamResourceStorages.GetGoldCount() >= goldCost) && (teamResourceStorages.GetWoodCount() >= woodCost))
        {
            building.recruiter.AddUnitToQueue(unit);
            teamResourceStorages.UpdateResCount(ResourceType.Gold, (int)-goldCost);
            teamResourceStorages.UpdateResCount(ResourceType.Wood, (int)-woodCost);
        }
    }

    public override void Deactivate()
    {
    }

    public override void Setup(SelectableObject selectedObject)
    {
        if (selectedObject is Building)
        {
            building = (Building)selectedObject;
        }
        teamResourceStorages = ResourceHandler.GetTeamStorage(building.tag);
        goldCost = unit.GetComponent<Unit>().GetBaseStats().baseStats.goldCost;
        woodCost = unit.GetComponent<Unit>().GetBaseStats().baseStats.woodCost;

    }

    public float GetGoldCost() { return goldCost; }
    public float GetWoodCost() { return woodCost; }
}
