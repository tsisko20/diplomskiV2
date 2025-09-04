using RTS.InputManager;
using RTS.Objects.Units;
using RTS.UI;
using UnityEngine;

public class DeadUnitState : UnitState
{
    private float deathTime;
    public DeadUnitState(Unit _unit, UnitStateMachine _stateMachine) : base(_unit, _stateMachine)
    {
    }

    public override void EnterState()
    {
        unit.navAgent.enabled = false;
        unit.target = null;
        Transform movingRange = unit.transform.Find("MovingRange");
        movingRange.gameObject.SetActive(false);
        unit.animator.ChangeAnimation(UnitStates.Dead);
        unit.Deselect();
        if (InputHandler.GetSelectableObjects().Remove(unit.GetComponent<Transform>()))
        {
            SelectedObjectUI.UpdateUI(InputHandler.GetSelectableObjects());
        }
    }

    public override void ExitState()
    {
    }

    public override void Update()
    {

    }
}
