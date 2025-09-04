using RTS.InputManager;
using RTS.Objects.Buildings;
using RTS.Objects.Units;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ConstructionState
{
    Inactive,
    Moving,
    Rotating
}
public class BuildingConstructor : MonoBehaviour
{
    public static BuildingConstructor instance { get; private set; }
    public Camera cam;

    public InputAction pointerPosition;
    public InputAction leftClick;
    public InputAction rightClick;
    public InputAction cancelInput;

    public LayerMask terrainLayer;
    public LayerMask buildingLayer;
    public Color validColor;
    public Color invalidColor;
    public float rotationSpeed = 0.5f;

    ConstructionState _state;
    Color _initColor;
    Vector3 _initEulers;
    Vector2 _initPointerPos;
    Building _building;
    TeamResourceStorages teamResourceStorages;

    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        pointerPosition.Enable();
        leftClick.Enable();
        rightClick.Enable();
        cancelInput.Enable();
    }
    private void OnDisable()
    {
        pointerPosition.Disable();
        leftClick.Disable();
        rightClick.Disable();
        cancelInput.Disable();
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
        instance._initColor = instance._building.GetColor();
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
        if (cancelInput.WasPressedThisFrame())
        {
            CancelConstruction();
            return;
        }
        UpdateConstructionState();
        if (_state == ConstructionState.Moving)
        {
            MoveBuilding();
        }
        else if (_state == ConstructionState.Rotating)
        {
            RotateBuilding();
        }
        if (!CanConstructAtPosition())
        {
            _building.SetColor(invalidColor);
            return;
        }
        _building.SetColor(validColor);
        if (leftClick.WasPressedThisFrame())
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
        if (rightClick.WasPressedThisFrame())
        {
            _initPointerPos = pointerPosition.ReadValue<Vector2>();
            _initEulers = _building.transform.eulerAngles;
        }
        if (rightClick.ReadValue<float>() > 0)
        {
            _state = ConstructionState.Rotating;
            return;
        }
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
        var ray = cam.ScreenPointToRay(pointerPosition.ReadValue<Vector2>());
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

    void RotateBuilding()
    {
        var delta = pointerPosition.ReadValue<Vector2>().x - _initPointerPos.x;
        _building.transform.eulerAngles = _initEulers + delta * rotationSpeed * Vector3.down;
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
