using RTS;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitCostTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BasicObject baseObj;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.parent.name == "AbilityGrid")
            Tooltip.Show($"Gold Cost: {baseObj.baseStats.goldCost} \n Wood Cost: {baseObj.baseStats.woodCost}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.parent.name == "AbilityGrid")
            Tooltip.Hide();
    }
}
