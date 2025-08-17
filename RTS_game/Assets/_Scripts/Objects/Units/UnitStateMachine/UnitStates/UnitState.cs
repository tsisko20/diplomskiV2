using RTS.Objects.Units;
using UnityEngine;

namespace RTS.Objects.Units
{
    public class UnitState
    {
        protected Unit unit;
        protected UnitStateMachine unitStateMachine;

        public UnitState(Unit _unit, UnitStateMachine _unitStateMachine)
        {
            unit = _unit;
            unitStateMachine = _unitStateMachine;
        }
        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void Update() { }
    }
}

