using RTS.Objects.Units;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ResourceGatherer : MonoBehaviour
{
    IGatherable gatheringResource;

    private Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void Update()
    {
        if (unit.target is IGatherable)
        {
            gatheringResource = unit.target as IGatherable;
        }
        else
        {
            if (gatheringResource != null)
            {
                gatheringResource = null;
            }
        }
        if (gatheringResource != null)
        {
            Collider targetCollider = gatheringResource.GetTransform().GetComponent<Collider>();
            Vector3 closestPoint = targetCollider.ClosestPoint(gatheringResource.GetTransform().position);
            float distance = Vector3.Distance(unit.transform.position, closestPoint);
            Debug.Log(distance);
            if (distance > 1)
            {
                unit.MoveTo(gatheringResource.GetTransform().position);
            }
            else
            {
                if (unit.state == UnitState.Walking)
                    unit.StopMoving();
                unit.state = UnitState.Gathering;
            }
        }
    }
}
