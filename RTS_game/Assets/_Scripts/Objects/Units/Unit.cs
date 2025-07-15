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
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : SelectableObject, IAttackable
    {
        public NavMeshAgent navAgent;
        public UnitAnimator animator;
        public bool isWalking = false;
        public BasicUnit unitStats;
        [SerializeField] private MovingRange movingRange;
        CombatBehaviour combatBehaviour;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Setup()
        {

            animator = GetComponent<UnitAnimator>();
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.speed = unitStats.GetMoveSpeed();
            combatBehaviour = GetComponent<CombatBehaviour>();
            SetTeam();
            SetMaterial();
            health = unitStats.baseStats.health;
        }



        // Update is called once per frame
        void Update()
        {
            if (isWalking && !navAgent.pathPending)
            {
                if (navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    StopMoving();
                }
                else
                {
                    foreach (Unit unitInMovingRange in movingRange.GetUnitsInMovingRange())
                    {

                        if (unitInMovingRange.GetCurrentDestination() == GetCurrentDestination() && unitInMovingRange.isWalking == false)
                        {
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
            InputManager.InputHandler.instance.selectedObjects.Remove(gameObject.GetComponent<Transform>());
            navAgent.isStopped = true;
            navAgent.ResetPath();
            animator.PlayDieAnimation();
            Collider col = GetComponent<Collider>();
            if (col) col.enabled = false;
            Transform movingRange = transform.Find("MovingRange");
            movingRange.gameObject.SetActive(false);
            //Destroy(gameObject);
        }
        public override bool IsDead() => health <= 0;
        public void MoveUnit(Vector3 destination)
        {
            navAgent.isStopped = false;
            isWalking = true;
            navAgent.SetDestination(destination);
            animator.PlayWalkingAnimation(true);
        }
        public void StopMoving()
        {
            animator.PlayWalkingAnimation(false);
            isWalking = false;
            navAgent.isStopped = true;
            combatBehaviour = GetComponent<CombatBehaviour>();
            if (combatBehaviour != null)
                combatBehaviour.SetAggressive(true);
        }
        public void Attack()
        {

        }

        public Transform GetTransform() => transform;

    }




}

