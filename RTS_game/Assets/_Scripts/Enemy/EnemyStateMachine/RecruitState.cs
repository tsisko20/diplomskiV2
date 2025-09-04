using RTS.Ability;
using RTS.Enemy;
using RTS.Objects;
using RTS.Objects.Buildings;

public class RecruitState : EnemyState
{
    int workersNeeded;
    int archersNeeded;
    public RecruitState(EnemyContext _enemyContext, EnemyStateMachine _stateMachine) : base(_enemyContext, _stateMachine)
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
        RecruitUnits();
    }

    void RecruitUnits()
    {
        if (CheckIfResStorageExist())
        {
            Recruiter recruiter = enemyContext.resourceStoragesParent.GetChild(0).GetComponent<Recruiter>();
            int currRecruit = 0;
            if (recruiter.currentRecruit != null)
                currRecruit = 1;
            workersNeeded = enemyContext.maxWorkersCount - (enemyContext.workersParent.childCount + recruiter.unitQueue.Count + currRecruit);
            if (workersNeeded != 0)
                RecruitFarmers();
        }
        if (CheckIfBarracksExist())
        {
            Recruiter recruiter = enemyContext.barracksParent.GetChild(0).GetComponent<Recruiter>();
            int currRecruit = 0;
            if (recruiter.currentRecruit != null)
                currRecruit = 1;
            archersNeeded = enemyContext.maxArchersCount - (enemyContext.archersParent.childCount + recruiter.unitQueue.Count + currRecruit);
            if (archersNeeded != 0)
                RecruitArchers();
        }
        if (workersNeeded <= 0 && archersNeeded <= 0 && CheckIfBarracksExist())
        {
            stateMachine.ChangeState(stateMachine.economyState);
        }
    }

    bool CheckIfResStorageExist()
    {
        if (enemyContext.resourceStoragesParent.childCount == 0)
        {
            stateMachine.ChangeState(stateMachine.buildState);
            return false;
        }
        else
            if (enemyContext.resourceStoragesParent.GetChild(0).GetComponent<Building>().state == BuildingState.Finished)
            return true;
        return false;
    }

    bool CheckIfBarracksExist()
    {
        if (enemyContext.barracksParent.childCount == 0)
        {
            if (workersNeeded == 0)
                stateMachine.ChangeState(stateMachine.buildState);
            return false;
        }
        else
            if (enemyContext.barracksParent.GetChild(0).GetComponent<Building>().state == BuildingState.Finished)
            return true;
        return false;
    }

    void RecruitFarmers()
    {
        if (enemyContext.workersParent.childCount < enemyContext.maxWorkersCount)
        {
            SelectableObject resourceStorage = enemyContext.resourceStoragesParent.GetChild(0).GetComponent<SelectableObject>();
            AbilityHolder recruitFarmer = enemyContext.FindAbility("Recruit Farmer", resourceStorage);
            RecruitUnitAbility recruitFarmerAbility = recruitFarmer.abilityInstance as RecruitUnitAbility;
            bool resetRequiredResources = false;
            for (int i = 0; i < workersNeeded; i++)
            {
                if (enemyContext.enemyStorage.GetWoodCount() >= recruitFarmerAbility.GetWoodCost() && enemyContext.enemyStorage.GetGoldCount() >= recruitFarmerAbility.GetGoldCost())
                    recruitFarmer.ActivateAbility();
                else
                {
                    enemyContext.goldRequired = recruitFarmerAbility.GetGoldCost() * (workersNeeded - i);
                    enemyContext.woodRequired = recruitFarmerAbility.GetWoodCost() * (workersNeeded - i);
                    stateMachine.ChangeState(stateMachine.economyState);
                    resetRequiredResources = false;
                    break;
                }
                resetRequiredResources = true;
            }
            if (resetRequiredResources)
            {
                enemyContext.goldRequired = 0;
                enemyContext.woodRequired = 0;
            }
        }
    }

    void RecruitArchers()
    {
        if (enemyContext.archersParent.childCount < enemyContext.maxArchersCount)
        {
            SelectableObject barracks = enemyContext.barracksParent.GetChild(0).GetComponent<SelectableObject>();
            AbilityHolder recruitArcher = enemyContext.FindAbility("Recruit Archer", barracks);
            RecruitUnitAbility recruitArcherAbility = recruitArcher.abilityInstance as RecruitUnitAbility;
            bool resetRequiredResources = false;
            for (int i = 0; i < archersNeeded; i++)
            {
                if (enemyContext.enemyStorage.GetWoodCount() >= recruitArcherAbility.GetWoodCost() && enemyContext.enemyStorage.GetGoldCount() >= recruitArcherAbility.GetGoldCost())
                    recruitArcher.ActivateAbility();
                else
                {
                    enemyContext.goldRequired = recruitArcherAbility.GetGoldCost() * (archersNeeded - i);
                    enemyContext.woodRequired = recruitArcherAbility.GetWoodCost() * (archersNeeded - i);
                    stateMachine.ChangeState(stateMachine.economyState);
                    resetRequiredResources = false;
                    break;
                }
                resetRequiredResources = true;
            }
            if (resetRequiredResources)
            {
                enemyContext.goldRequired = 0;
                enemyContext.woodRequired = 0;
            }
        }
    }
}
