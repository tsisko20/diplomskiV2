using RTS.Enemy;
using UnityEngine;

[RequireComponent(typeof(EnemyContext))]
public class EnemyHandler : MonoBehaviour
{
    public EnemyContext enemyContext;
    public EnemyStateMachine stateMachine;

    private void Awake()
    {
        enemyContext = GetComponent<EnemyContext>();
    }
    private void Start()
    {
        stateMachine = new EnemyStateMachine(enemyContext);
    }
    private void Update()
    {
        stateMachine.currentState.Update();
    }
}
