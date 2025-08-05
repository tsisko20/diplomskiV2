using JetBrains.Annotations;
using RTS.InputManager;
using RTS.Units;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.CanvasScaler;

namespace RTS.Objects.Units
{
    public enum UnitState
    {
        Idle,
        Walking,
        Attacking,
        Gathering,
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
        [SerializeField] private MovingRange movingRange;
        public CombatBehaviour combatBehaviour;
        public ResourceGatherer resourceGatherer;
        public Vector3 destination;
        public UnitState state;
        public ITargetable target;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Setup()
        {

            navAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<UnitAnimator>();
            combatBehaviour = GetComponent<CombatBehaviour>();
            resourceGatherer = GetComponent<ResourceGatherer>();
            navAgent.speed = unitStats.GetMoveSpeed();
            SetTeam();
            SetMaterial();
            health = unitStats.baseStats.health;
        }



        // Update is called once per frame
        void Update()
        {
            if (state == UnitState.Walking && !navAgent.pathPending && navAgent.hasPath)
            {
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    Debug.Log("stop 1");
                    StopMoving();
                }
                else
                {
                    foreach (Unit unitInMovingRange in movingRange.GetUnitsInMovingRange())
                    {

                        if (unitInMovingRange.GetCurrentDestination() == GetCurrentDestination() && unitInMovingRange.state != UnitState.Walking)
                        {
                            Debug.Log("stop 2");
                            StopMoving();
                        }
                    }
                }
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
            ChangeState(UnitState.Dead);
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
            ChangeState(UnitState.Walking);
            navAgent.SetDestination(destination);
        }
        public void StopMoving()
        {
            Debug.Log("stop moving");
            ChangeState(UnitState.Idle);
            navAgent.isStopped = true;
            combatBehaviour = GetComponent<CombatBehaviour>();
        }

        public Transform GetTransform() => transform;

        public void ChangeState(UnitState _state)
        {
            state = _state;
            animator.ChangeAnimation(state);
        }

    }




}

