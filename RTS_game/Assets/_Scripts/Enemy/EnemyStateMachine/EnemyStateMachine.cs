namespace RTS.Enemy
{
    public class EnemyStateMachine
    {
        public EnemyState currentState;

        public EconomyState economyState;
        public AttackState attackState;
        public BuildState buildState;
        public RecruitState recruitState;

        public EnemyStateMachine(EnemyContext enemyContext)
        {
            economyState = new EconomyState(enemyContext, this);
            attackState = new AttackState(enemyContext, this);
            buildState = new BuildState(enemyContext, this);
            recruitState = new RecruitState(enemyContext, this);
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

