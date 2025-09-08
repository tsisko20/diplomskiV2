using RTS.InputManager;
using RTS.Objects.Buildings;
using RTS.Objects.Units;
using UnityEngine;

public enum ConstructionState
{
    Inactive,
    Moving,
}

public class BuildingConstructor : MonoBehaviour
{
    public static BuildingConstructor instance { get; private set; }
    public Camera cam;

    public LayerMask terrainLayer;
    public LayerMask buildingLayer;
    public Color validColor;
    public Color invalidColor;

    ConstructionState _state;
    Color _initColor;
    Building _building;
    TeamResourceStorages teamResourceStorages;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        teamResourceStorages = ResourceHandler.instance.playerResStorage;
    }

    public static void EnterConstructionMode(GameObject prefab)
    {
        instance.ClearExistingBuilding();
        instance._state = ConstructionState.Moving;
        var clone = Instantiate(prefab, instance.MouseToFloorPoint(), prefab.transform.rotation);
        instance._building = clone.GetComponent<Building>();
        instance._building.state = BuildingState.Init;
        instance._building.navMeshObstacle.enabled = false;
        instance._building.enabled = false;
    }

    private void Update()
    {
        if (_state != ConstructionState.Inactive)
        {
            ProcessConstructionMode();
        }
    }

    void ProcessConstructionMode()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelConstruction();
            return;
        }

        UpdateConstructionState();

        if (_state == ConstructionState.Moving)
        {
            MoveBuilding();
        }

        if (!CanConstructAtPosition())
        {
            _building.SetColor(invalidColor);
            return;
        }

        _building.SetColor(validColor);

        if (Input.GetMouseButtonDown(0))
        {
            ConstructBuilding();
        }
    }

    void CancelConstruction()
    {
        ClearExistingBuilding();
        ExitConstructionMode();
    }

    void UpdateConstructionState()
    {
        _state = ConstructionState.Moving;
    }

    void ClearExistingBuilding()
    {
        if (_building != null)
        {
            Destroy(_building.gameObject);
        }
    }

    void ExitConstructionMode()
    {
        _building = null;
        _state = ConstructionState.Inactive;
    }

    Vector3 MouseToFloorPoint()
    {
        RaycastHit[] rayHits = new RaycastHit[1];
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        var hitCount = Physics.RaycastNonAlloc(ray, rayHits, Mathf.Infinity, terrainLayer);
        if (hitCount == 0)
        {
            return Vector3.zero;
        }
        return rayHits[0].point;
    }

    void MoveBuilding()
    {
        _building.transform.position = MouseToFloorPoint();
    }

    bool CanConstructAtPosition()
    {
        Collider[] overlaps = new Collider[2];
        var objectCenter = _building.CenterPoint();
        var extents = _building.ColliderWorldSize();
        var collisionCount = Physics.OverlapBoxNonAlloc(objectCenter, extents / 2, overlaps, _building.transform.rotation, buildingLayer);
        return collisionCount == 1;
    }

    void ConstructBuilding()
    {
        _building.meshRenderer.material.color = _initColor;
        _building.SetTeam("Player");
        _building.enabled = true;
        _building.navMeshObstacle.enabled = true;

        foreach (Transform selected in InputHandler.GetSelectableObjects())
        {
            Unit unit = selected.GetComponent<Unit>();
            unit.UpdateState(_building.gameObject);
        }

        float goldCost = _building.GetBaseStats().baseStats.goldCost;
        float woodCost = _building.GetBaseStats().baseStats.woodCost;
        teamResourceStorages.UpdateResCount(ResourceType.Gold, (int)-goldCost);
        teamResourceStorages.UpdateResCount(ResourceType.Wood, (int)-woodCost);

        ExitConstructionMode();
    }
}
