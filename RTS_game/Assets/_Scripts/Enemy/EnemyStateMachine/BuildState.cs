using RTS.Objects.Units;
using UnityEngine;

namespace RTS.Enemy
{
    public class BuildState : EnemyState
    {
        public BuildState(EnemyContext _enemyContext, EnemyStateMachine _stateMachine) : base(_enemyContext, _stateMachine)
        {
        }

        public override void EnterState()
        {
            ConstructBuildings();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void Update()
        {
            base.Update();
        }

        void ConstructBuildings()
        {
            if (enemyContext.resourceStoragesParent.childCount == 0)
            {
                ConstructResourceStorage();
            }
        }

        void ConstructResourceStorage()
        {
            Unit workerUnit = enemyContext.workersParent.GetChild(0).GetComponent<Unit>();
        }
    }
}

