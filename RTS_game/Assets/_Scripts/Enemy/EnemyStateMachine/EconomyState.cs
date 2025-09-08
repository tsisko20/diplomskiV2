using RTS.Objects.Buildings;
using RTS.Objects.Units;
using UnityEngine;

namespace RTS.Enemy
{
    public class EconomyState : EnemyState
    {
        public EconomyState(EnemyContext _enemyContext, EnemyStateMachine _stateMachine) : base(_enemyContext, _stateMachine)
        {
        }

        public override void EnterState()
        {
            if (enemyContext.resourceStoragesParent.childCount != 0)
            {
                if (enemyContext.resourceStoragesParent.GetChild(0).GetComponent<Building>().state == BuildingState.Finished)
                    GatherResources();
            }
            else
            {
                if (RequiredResCollected())
                {
                    stateMachine.ChangeState(stateMachine.buildState);
                }
                else
                {
                    enemyContext.isOutOfMoney = true;
                    stateMachine.ChangeState(stateMachine.attackState);
                }
            }
        }

        public override void Update()
        {
            if (enemyContext.resourceStoragesParent.childCount == 0)
            {
                stateMachine.ChangeState(stateMachine.buildState);
            }
            if (enemyContext.woodRequired == 0 && enemyContext.goldRequired == 0 && enemyContext.maxArchersCount <= enemyContext.archersParent.childCount)
            {
                stateMachine.ChangeState(stateMachine.attackState);
                return;
            }
            if (RequiredResCollected())
            {
                stateMachine.ChangeState(stateMachine.recruitState);
            }
        }

        private void GatherResources()
        {
            int workerCount = enemyContext.workersParent.childCount;
            for (int i = 0; i < workerCount; i++)
            {
                Unit enemyWorker = enemyContext.workersParent.GetChild(i).GetComponent<Unit>();
                Vector3 unitLocation = enemyWorker.transform.position;
                if (i % 2 == 0)
                {
                    enemyWorker.UpdateState(ResourceGatherer.FindNearestResource(ResourceType.Gold, ref unitLocation));
                }
                else
                {
                    enemyWorker.UpdateState(ResourceGatherer.FindNearestResource(ResourceType.Wood, ref unitLocation));
                }
            }
        }

        private bool RequiredResCollected()
        {
            if (enemyContext.enemyStorage.GetWoodCount() >= enemyContext.woodRequired && enemyContext.enemyStorage.GetGoldCount() >= enemyContext.goldRequired)
                return true;
            return false;
        }
    }
}
