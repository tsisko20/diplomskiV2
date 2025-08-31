using RTS.Objects.Units;
using UnityEngine;

namespace RTS.Enemy
{
    public class EnemyState
    {
        //protected Unit unit;
        protected EnemyStateMachine stateMachine;
        protected EnemyContext enemyContext;
        public EnemyState(EnemyContext _enemyContext, EnemyStateMachine _stateMachine)
        {
            enemyContext = _enemyContext;
            this.stateMachine = _stateMachine;
        }
        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void Update() { }
    }
}
