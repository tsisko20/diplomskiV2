namespace RTS.Objects.Units
{
    public class UnitState
    {
        protected Unit unit;
        protected UnitStateMachine stateMachine;

        public UnitState(Unit _unit, UnitStateMachine _stateMachine)
        {
            unit = _unit;
            this.stateMachine = _stateMachine;
        }
        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void Update() { }
    }
}

