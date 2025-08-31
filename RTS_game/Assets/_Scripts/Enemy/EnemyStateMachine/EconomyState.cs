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
            ManageResources();
        }

        public override void ExitState()
        {
        }

        public override void Update()
        {
            if (RequiredResCollected())
            {
                stateMachine.ChangeState(stateMachine.recruitState);
            }
        }

        private void ManageResources()
        {
            Debug.Log("manage resources");
            int workerCount = enemyContext.workersParent.childCount;
            int firstWorkerHalf = workerCount / 2;
            int secondWorkerHalf = workerCount - firstWorkerHalf;
            for (int i = 0; i < secondWorkerHalf; i++)
            {
                Unit enemyWorker = enemyContext.workersParent.GetChild(i).GetComponent<Unit>();
                Vector3 unitLocation = enemyWorker.transform.position;
                enemyWorker.UpdateState(ResourceGatherer.FindNearestResource(ResourceType.Gold, ref unitLocation));
            }
            for (int i = secondWorkerHalf; i < workerCount; i++)
            {
                Unit enemyWorker = enemyContext.workersParent.GetChild(i).GetComponent<Unit>();
                Vector3 unitLocation = enemyWorker.transform.position;
                enemyWorker.UpdateState(ResourceGatherer.FindNearestResource(ResourceType.Wood, ref unitLocation));
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
