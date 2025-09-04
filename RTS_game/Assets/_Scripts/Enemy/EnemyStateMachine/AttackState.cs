using RTS.Objects;
using RTS.Objects.Units;
using UnityEngine;

namespace RTS.Enemy
{
    public class AttackState : EnemyState
    {
        Transform attackTarget;
        bool endGame = false;
        public AttackState(EnemyContext _enemyContext, EnemyStateMachine _stateMachine) : base(_enemyContext, _stateMachine)
        {
        }

        public override void EnterState()
        {

        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void Update()
        {
            if (attackTarget != null)
            {
                SelectableObject selectable = attackTarget.GetComponent<SelectableObject>();
                if (selectable.IsDead() == true)
                    attackTarget = null;
            }
            if (enemyContext.isOutOfMoney)
            {
                endGame = true;
                FindAttackTarget();
                if (attackTarget != null)
                    foreach (Transform worker in enemyContext.workersParent)
                    {
                        Unit workerUnit = worker.GetComponent<Unit>();
                        workerUnit.UpdateState(attackTarget.gameObject);
                    }
            }

            if (enemyContext.archersParent.childCount == 0 && endGame == false)
            {
                stateMachine.ChangeState(stateMachine.recruitState);
                return;
            }
            if (attackTarget != null && enemyContext.isOutOfMoney == false)
            {
                stateMachine.ChangeState(stateMachine.economyState);
                SelectableObject selectable = attackTarget.GetComponent<SelectableObject>();
                if (selectable.IsDead())
                    attackTarget = null;
            }
            if (endGame == false)
                FindAttackTarget();

            if (attackTarget != null)
                foreach (Transform archer in enemyContext.archersParent)
                {
                    Unit archerUnit = archer.GetComponent<Unit>();
                    archerUnit.UpdateState(attackTarget.gameObject);
                }
            stateMachine.ChangeState(stateMachine.economyState);
        }

        void FindAttackTarget()
        {
            if (attackTarget == null)
                attackTarget = GetPlayerTarget("Player/Units/Archers");
            if (attackTarget == null)
                attackTarget = GetPlayerTarget("Player/Units/Workers");
            if (attackTarget == null)
                attackTarget = GetPlayerTarget("Player/Buildings/Barracks");
            if (attackTarget == null)
                attackTarget = GetPlayerTarget("Player/Buildings/Resource Storages");
        }

        Transform GetPlayerTarget(string targetHierarchy)
        {
            Transform targetParent = GameObject.Find(targetHierarchy).transform;
            foreach (Transform possibleTarget in targetParent)
            {
                SelectableObject selectable = possibleTarget.GetComponent<SelectableObject>();
                if (selectable.IsDead() == false)
                    return possibleTarget;
            }
            return null;
        }
    }
}

