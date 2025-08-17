using JetBrains.Annotations;
using RTS.InputManager;
using RTS.Objects.Buildings;
using RTS.Units;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.CanvasScaler;

namespace RTS.Objects.Units
{
    public enum UnitStates
    {
        Idle,
        Walking,
        Attacking,
        Gathering,
        Repairing,
        Dead
    }
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(UnitAnimator))]
    [RequireComponent(typeof(CombatBehaviour))]
    public class Unit : SelectableObject, IAttackable
    {

        public NavMeshAgent navAgent;
        public UnitAnimator animator;
        public BasicUnit unitStats;
        public MovingRange movingRange;
        public CombatBehaviour combatBehaviour;
        public ResourceGatherer resourceGatherer;
        public BuildingConstructor buildingConstructor;
        public Vector3 destination;
        public UnitStates state;
        public GameObject target;
        public SkinnedMeshRenderer rend;
        private delegate void InteractWithTarget();

        public UnitStateMachine stateMachine;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Setup()
        {
            navAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<UnitAnimator>();
            combatBehaviour = GetComponent<CombatBehaviour>();
            resourceGatherer = GetComponent<ResourceGatherer>();
            navAgent.speed = unitStats.GetMoveSpeed();
            SetTeamByHierarchy();
            SetBuildingConstructor();
            rend.material.color = GetTeamColor();
            health = unitStats.baseStats.health;
            stateMachine = new UnitStateMachine(this);
        }

        private void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.currentState.Update();
            //if (state == UnitStates.Walking && !navAgent.pathPending && navAgent.hasPath)
            //{

            //    if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            //    {
            //        Debug.Log("stop 1");
            //        StopMoving();
            //    }
            //    else
            //    {
            //        foreach (Unit unitInMovingRange in movingRange.GetUnitsInMovingRange())
            //        {

            //            if (unitInMovingRange.GetCurrentDestination() == GetCurrentDestination() && unitInMovingRange.state != UnitStates.Walking && target == null)
            //            {
            //                Debug.Log("stop 2");
            //                StopMoving();
            //            }
            //        }
            //    }
            //}
        }

        public void UpdateState(GameObject _target)
        {

            target = _target;
            if (target.layer.Equals(6))
            {
                target = null;
                stateMachine.ChangeState(stateMachine.walkState);
                return;
            }
            if (target.GetComponent<IAttackable>() != null)
            {
                if (target.GetComponent<IAttackable>().GetTeam() != team)
                {
                    stateMachine.ChangeState(stateMachine.attackState);
                    return;

                }
                else
                {
                    if (target.GetComponent<Building>() != null && unitStats.unitType == UnitType.Worker)
                    {
                        stateMachine.ChangeState(stateMachine.constructState);
                        return;
                    }
                    else
                    {
                        stateMachine.ChangeState(stateMachine.walkState);
                        return;
                    }
                }
            }
            if (target.GetComponent<IGatherable>() != null && unitStats.unitType == UnitType.Worker)
            {
                Debug.Log("moze se gatherati");
                stateMachine.ChangeState(stateMachine.gatherState);
                return;
            }
        }

        public override string GetObjectName()
        {
            return unitStats.unitName;
        }

        public override BasicObject GetBaseStats()
        {
            return unitStats;
        }

        public Vector3 GetCurrentDestination()
        {
            return navAgent.destination;
        }



        protected override void Die()
        {
            InputManager.InputHandler.instance.GetSelectableObjects().Remove(gameObject.GetComponent<Transform>());
            navAgent.isStopped = true;
            navAgent.ResetPath();
            ChangeState(UnitStates.Dead);
            Collider col = GetComponent<Collider>();
            if (col) col.enabled = false;
            Transform movingRange = transform.Find("MovingRange");
            movingRange.gameObject.SetActive(false);
            //Destroy(gameObject);
        }
        public override bool IsDead() => health <= 0;
        public void MoveTo(Vector3 destination)
        {
            navAgent.isStopped = false;
            ChangeState(UnitStates.Walking);
            navAgent.SetDestination(destination);
        }
        public void StopMoving()
        {
            navAgent.isStopped = true;
            combatBehaviour = GetComponent<CombatBehaviour>();
        }

        public Transform GetTransform() => transform;

        public void ChangeState(UnitStates _state)
        {
            state = _state;
            animator.ChangeAnimation(state);
        }

        public override void SetColor(Color color)
        {
            rend.material.color = color;
        }

        private void SetBuildingConstructor()
        {
            switch (team)
            {
                case Team.Player:
                    buildingConstructor = GameObject.Find("Player").GetComponent<BuildingConstructor>(); break;
                case Team.Enemy:
                    buildingConstructor = GameObject.Find("Enemy").GetComponent<BuildingConstructor>(); break;
            }
        }


    }
}

