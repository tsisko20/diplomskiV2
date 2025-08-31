using RTS.Ability;
using RTS.Enemy;
using RTS.Objects;
using UnityEngine;

public class RecruitState : EnemyState
{
    int maxWorkersCount = 4;
    int maxArchersCount = 4;
    public RecruitState(EnemyContext _enemyContext, EnemyStateMachine _stateMachine) : base(_enemyContext, _stateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("recruit state");
        RecruitUnits();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Update()
    {
        base.Update();
    }

    void RecruitUnits()
    {
        RecruitFarmers();
        RecruitArchers();
    }

    void RecruitFarmers()
    {
        if (enemyContext.workersParent.childCount < maxWorkersCount)
        {
            int workersNeeded = maxWorkersCount - enemyContext.workersParent.childCount;
            if (enemyContext.resourceStoragesParent.childCount == 0)
            {
                Debug.Log("trebam farm house");
                stateMachine.ChangeState(stateMachine.buildState);
                return;
            }
            else
            {
                SelectableObject resourceStorage = enemyContext.resourceStoragesParent.GetChild(0).GetComponent<SelectableObject>();
                AbilityHolder recruitFarmer = enemyContext.FindAbility("Recruit Farmer", resourceStorage);
                RecruitUnitAbility recruitFarmerAbility = recruitFarmer.abilityInstance as RecruitUnitAbility;
                for (int i = 0; i < workersNeeded; i++)
                {
                    if (enemyContext.enemyStorage.GetWoodCount() >= recruitFarmerAbility.GetWoodCost() && enemyContext.enemyStorage.GetGoldCount() >= recruitFarmerAbility.GetGoldCost())
                        recruitFarmer.ActivateAbility();
                    else
                    {
                        enemyContext.goldRequired = recruitFarmerAbility.GetGoldCost() * workersNeeded - i;
                        enemyContext.woodRequired = recruitFarmerAbility.GetWoodCost() * workersNeeded - i;
                        stateMachine.ChangeState(stateMachine.economyState);
                        break;
                    }
                }
            }
        }
    }

    void RecruitArchers()
    {
        if (enemyContext.archersParent.childCount < maxArchersCount)
        {
            int archersNeeded = maxArchersCount - enemyContext.archersParent.childCount;
            if (enemyContext.barracksParent.childCount == 0)
            {
                Debug.Log("trebam barracks");
                stateMachine.ChangeState(stateMachine.buildState);
                return;
            }
            else
            {
                SelectableObject barracks = enemyContext.barracksParent.GetChild(0).GetComponent<SelectableObject>();
                AbilityHolder recruitArcher = enemyContext.FindAbility("Recruit Archer", barracks);
                RecruitUnitAbility recruitArcherAbility = recruitArcher.abilityInstance as RecruitUnitAbility;
                for (int i = 0; i < archersNeeded; i++)
                {
                    Debug.Log(enemyContext.enemyStorage.GetWoodCount() + " " + recruitArcherAbility.GetWoodCost());
                    Debug.Log(enemyContext.enemyStorage.GetGoldCount() + " " + recruitArcherAbility.GetGoldCost());
                    if (enemyContext.enemyStorage.GetWoodCount() >= recruitArcherAbility.GetWoodCost() && enemyContext.enemyStorage.GetGoldCount() >= recruitArcherAbility.GetGoldCost())
                        recruitArcher.ActivateAbility();
                    else
                    {
                        enemyContext.goldRequired = recruitArcherAbility.GetGoldCost() * archersNeeded - i;
                        enemyContext.woodRequired = recruitArcherAbility.GetWoodCost() * archersNeeded - i;
                        stateMachine.ChangeState(stateMachine.economyState);
                        break;
                    }
                }
            }
        }
    }
}
