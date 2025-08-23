using UnityEngine;

namespace RTS.Objects.Units
{
    public class UnitStateMachine
    {
        public UnitState currentState;

        public IdleUnitState idleState;
        public WalkUnitState walkState;
        public AttackUnitState attackState;
        public GatherUnitState gatherState;
        public ConstructUnitState constructState;
        public DeadUnitState deadState;


        public UnitStateMachine(Unit _unit)
        {
            idleState = new IdleUnitState(_unit, this);
            walkState = new WalkUnitState(_unit, this);
            attackState = new AttackUnitState(_unit, this);
            gatherState = new GatherUnitState(_unit, this);
            constructState = new ConstructUnitState(_unit, this);
            deadState = new DeadUnitState(_unit, this);
            currentState = idleState;
            currentState.EnterState();
        }

        public void ChangeState(UnitState newState)
        {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

    }
}

