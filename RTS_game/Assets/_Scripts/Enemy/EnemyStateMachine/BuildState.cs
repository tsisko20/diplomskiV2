using RTS.Ability;
using RTS.Objects.Buildings;
using RTS.Objects.Units;
using UnityEngine;

namespace RTS.Enemy
{
    public class BuildState : EnemyState
    {
        Building currentConstruction;
        Unit builder;
        bool buildInProgress;
        public BuildState(EnemyContext _enemyContext, EnemyStateMachine _stateMachine) : base(_enemyContext, _stateMachine)
        {
        }

        public override void EnterState()
        {
            ConstructBuildings();
        }

        public override void Update()
        {
            if (currentConstruction == null)
            {
                stateMachine.ChangeState(stateMachine.economyState);
            }
            else
            {
                if (currentConstruction.GetCurrentHealth() == currentConstruction.GetBaseStats().baseStats.health)
                {
                    buildInProgress = false;
                    currentConstruction = null;
                }
            }
            if (currentConstruction != null && (builder == null || builder.IsDead()))
            {
                buildInProgress = false;
                foreach (Transform unit in enemyContext.workersParent)
                {
                    Unit possibleBuilder = unit.GetComponent<Unit>();
                    if (possibleBuilder.IsDead() == false)
                    {
                        builder = possibleBuilder;
                        builder.UpdateState(currentConstruction.gameObject);
                        buildInProgress = true;
                        break;
                    }
                }
            }
            if (currentConstruction != null && builder != null && builder.IsDead() == false && buildInProgress == false)
            {
                builder.UpdateState(currentConstruction.gameObject);
                buildInProgress = true;
            }
        }

        void ConstructBuildings()
        {
            if (currentConstruction == null)
            {
                if (enemyContext.resourceStoragesParent.childCount == 0)
                {
                    ConstructResourceStorage();
                }
                if (enemyContext.barracksParent.childCount == 0 && currentConstruction == null)
                {
                    ConstructBarracks();
                }
            }
        }

        void ConstructResourceStorage()
        {
            Unit workerUnit = enemyContext.workersParent.GetChild(0).GetComponent<Unit>();
            AbilityHolder buildResourceStorage = enemyContext.FindAbility("Place Farm House", workerUnit);
            PlaceBuildingAbility buildResourceStorageAbility = buildResourceStorage.abilityInstance as PlaceBuildingAbility;
            if (enemyContext.enemyStorage.GetWoodCount() >= buildResourceStorageAbility.GetWoodCost() && enemyContext.enemyStorage.GetGoldCount() >= buildResourceStorageAbility.GetGoldCost())
            {
                enemyContext.buildLocation = enemyContext.resourceStorageConstructLocation;
                buildResourceStorageAbility.Activate();
                currentConstruction = enemyContext.resourceStoragesParent.GetChild(0).GetComponent<Building>();
                builder = workerUnit;
                workerUnit.UpdateState(enemyContext.resourceStoragesParent.GetChild(0).gameObject);
                enemyContext.goldRequired = 0;
                enemyContext.woodRequired = 0;
            }
            else
            {
                enemyContext.goldRequired = buildResourceStorageAbility.GetGoldCost();
                enemyContext.woodRequired = buildResourceStorageAbility.GetWoodCost();
                if (currentConstruction != null)
                    stateMachine.ChangeState(stateMachine.economyState);
            }

        }

        void ConstructBarracks()
        {
            Unit workerUnit = enemyContext.workersParent.GetChild(0).GetComponent<Unit>();
            AbilityHolder buildBarracks = enemyContext.FindAbility("Place Barracks", workerUnit);
            PlaceBuildingAbility buildBarracksAbility = buildBarracks.abilityInstance as PlaceBuildingAbility;
            if (enemyContext.enemyStorage.GetWoodCount() >= buildBarracksAbility.GetWoodCost() && enemyContext.enemyStorage.GetGoldCount() >= buildBarracksAbility.GetGoldCost())
            {
                enemyContext.buildLocation = enemyContext.barracksConstructLocation;
                buildBarracksAbility.Activate();
                currentConstruction = enemyContext.barracksParent.GetChild(0).GetComponent<Building>();
                builder = workerUnit;
                workerUnit.UpdateState(enemyContext.barracksParent.GetChild(0).gameObject);
                enemyContext.goldRequired = 0;
                enemyContext.woodRequired = 0;
            }
            else
            {
                enemyContext.goldRequired = buildBarracksAbility.GetGoldCost();
                enemyContext.woodRequired = buildBarracksAbility.GetWoodCost();
                stateMachine.ChangeState(stateMachine.economyState);
            }
        }
    }
}

