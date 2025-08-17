using RTS.Ability;
using RTS.Objects;
using RTS.Objects.Buildings;
using RTS.Objects.Units;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Place Building Ability")]
public class PlaceBuildingAbility : AbilityBase
{
    [SerializeField] private GameObject building;
    private Unit unit;

    public override void Setup(SelectableObject selectedObject)
    {
        if (selectedObject is Unit)
        {
            unit = (Unit)selectedObject;
        }
    }

    public override void Activate()
    {
        unit.buildingConstructor.EnterConstructionMode(building);
    }

    public override void Deactivate()
    {
    }


}
