using RTS.Objects.Units;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace RTS.Enemy
{
    public class EnemyStateMachine
    {
        public EnemyState currentState;

        public EconomyState economyState;
        public AttackState attackState;
        public DefenseState defenseState;
        public BuildState buildState;
        public RecruitState recruitState;

        public EnemyStateMachine(EnemyContext enemyContext)
        {
            economyState = new EconomyState(enemyContext, this);
            attackState = new AttackState(enemyContext, this);
            defenseState = new DefenseState(enemyContext, this);
            buildState = new BuildState(enemyContext, this);
            recruitState = new RecruitState(enemyContext, this);
            Debug.Log("ulazi u economy state");
            currentState = economyState;
            currentState.EnterState();
        }

        public void ChangeState(EnemyState newState)
        {
            currentState.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    }
}

