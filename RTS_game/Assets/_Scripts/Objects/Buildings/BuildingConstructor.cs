using RTS.InputManager;
using RTS.Objects.Buildings;
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

    public void EnterConstructionMode(GameObject prefab)
    {
        ClearExistingBuilding();
        _state = ConstructionState.Moving;
        var clone = Instantiate(prefab, MouseToFloorPoint(), prefab.transform.rotation);
        _building = clone.GetComponent<Building>();
        _building.state = BuildingState.Init;
        _building.navMeshObstacle.enabled = false;
        _building.enabled = false;
        _initColor = _building.GetColor();
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
        _building.SetTeam(RTS.Team.Player);
        _building.enabled = true;
        _building.navMeshObstacle.enabled = true;
        ExitConstructionMode();
    }

}
