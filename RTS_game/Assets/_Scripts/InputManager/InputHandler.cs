using RTS.InputManager;
using RTS.Objects;
using RTS.Objects.Units;
using RTS.UI;
using RTS.Units;
using RTS.Units.Player;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
namespace RTS.InputManager
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance;
        public SelectedObjectUI selectedUI;
        private RaycastHit hit;
        public List<Transform> selectedObjects = new List<Transform>();
        [SerializeField] private bool isDragging = false;
        private Vector3 mousePos;
        private float dragThreshold = 10f;
        private bool mouseStartedOverUI = false;
        private const string SELECTION_SPRITE = "selectionSprite";
        CombatBehaviour combatBehaviour;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            instance = this;
            combatBehaviour = GetComponent<CombatBehaviour>();
        }
        private void Start()
        {
            selectedUI.UpdateSelectionUI(selectedObjects);
        }

        private void Update()
        {
            HandleUnitMovement();

        }

        private void OnGUI()
        {
            if (isDragging)
            {
                Rect rect = MultiSelect.GetScreenRect(mousePos, Input.mousePosition);
                MultiSelect.DrawScreenRect(rect, new Color(138f, 98f, 181f, 0.25f));
                MultiSelect.DrawScreenRectBorder(rect, 3, Color.blue);
            }
        }
        public void HandleUnitMovement()
        {
            // Start drag
            if (Input.GetMouseButtonDown(0))
            {

                mousePos = Input.mousePosition;
            }

            // Check for dragging
            if (Input.GetMouseButton(0))
            {
                if (Vector3.Distance(mousePos, Input.mousePosition) > dragThreshold)
                {
                    isDragging = true;
                }
            }

            // On release
            if (Input.GetMouseButtonUp(0))
            {
                if (isDragging)
                {
                    // Multi-selekcija
                    DeselectObjects();
                    foreach (Transform unitType in Player.UnitManager.instance.playerUnits)
                    {
                        foreach (Transform unit in unitType)
                        {
                            SelectableObject selectable = unit.GetComponent<SelectableObject>();
                            if (selectable != null && IsWithinSelectedBounds(unit))
                            {
                                SelectObject(selectable, true);
                            }
                        }
                    }
                }
                else
                {
                    // Klik selekcija
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        SelectableObject selectable = hit.transform.GetComponent<SelectableObject>();
                        if (selectable != null)
                        {
                            if (!Input.GetKey(KeyCode.LeftShift))
                                DeselectObjects();

                            SelectObject(selectable, Input.GetKey(KeyCode.LeftShift));
                        }
                        else
                        {
                            DeselectObjects();
                        }
                    }
                }

                isDragging = false;
                selectedUI.UpdateSelectionUI(selectedObjects);
            }

            // Desni klik: kretanje / napad
            if (Input.GetMouseButton(1) && HaveSelectedUnits())
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    IAttackable target = hit.transform.GetComponent<IAttackable>();
                    foreach (Transform selectedUnit in selectedObjects)
                    {
                        Unit unit = selectedUnit.GetComponent<Unit>();
                        if (unit == null || unit.IsDead() || unit.GetTeam() != Team.Player) continue;

                        CombatBehaviour cb = unit.GetComponent<CombatBehaviour>();

                        if (target != null && target.GetTeam() != Team.Player)
                        {
                            cb.SetAggressive(true);
                            cb.SetAggro(target);
                        }
                        else
                        {
                            unit.MoveUnit(hit.point);
                            cb.SetAggressive(false);
                        }
                    }
                }
            }
        }

        private void SelectObject(SelectableObject selectable, bool canMultiSelect = false)
        {
            if (!canMultiSelect)
            {
                DeselectObjects();
            }
            //Transform objTransform = selectable.transform;
            //if (selectedObjects.Contains(objTransform) == false)
            //{
            if (selectable.IsDead() == false)
            {
                selectedObjects.Add(selectable.transform);
                selectable.Select();
            }
            //}
        }

        private void DeselectObjects()
        {
            foreach (Transform obj in selectedObjects)
            {
                SelectableObject selectable = obj.GetComponent<SelectableObject>();
                if (selectable != null)
                {
                    selectable.Deselect();
                }

            }
            selectedObjects.Clear();
        }
        private bool IsWithinSelectedBounds(Transform selectable)
        {
            if (!isDragging)
            {
                return false;
            }
            Camera cam = Camera.main;
            Bounds vpBounds = MultiSelect.GetVPBounds(cam, mousePos, Input.mousePosition);
            return vpBounds.Contains(cam.WorldToViewportPoint(selectable.transform.position));
        }

        private bool HaveSelectedUnits()
        {
            return (selectedObjects.Count > 0);
        }
    }
}