using UnityEngine;

namespace RTS.Objects.Units
{
    public class UnitStateMachine
    {
        public UnitState currentState;
        public void Initialize(UnitState startingState)
        {
            currentState = startingState;
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

