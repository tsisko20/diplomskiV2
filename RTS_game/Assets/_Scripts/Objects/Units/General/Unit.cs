using JetBrains.Annotations;
using RTS.Buildings;
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
    public class Unit : SelectableObject, IAttackable
    {

        public NavMeshAgent navAgent;
        public UnitAnimator animator;
        public BasicUnit unitStats;
        public MovingRange movingRange;
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
            movingRange = transform.Find("MovingRange").GetComponent<MovingRange>();
            navAgent.speed = unitStats.GetMoveSpeed();
            SetTeamByHierarchy();
            SetBuildingConstructor();
            minimapIcon.color = GetTeamColor();
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
                if (target.tag != gameObject.tag)
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
                stateMachine.ChangeState(stateMachine.gatherState);
                return;
            }
        }

        protected override string GetInstanceDestination()
        {
            string parentFolder = unitStats.unitName + "s";
            string root = gameObject.tag;
            return $"{gameObject.tag}/Units/{parentFolder}";
        }

        public override string GetTeam() => gameObject.tag;
        public void TakeDamage(float damage)
        {

            float totalDamage = damage - (damage * (armor / 100));
            health -= totalDamage;
            if (health <= 0)
            {
                stateMachine.ChangeState(stateMachine.deadState);
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

        public void Heal(float amount)
        {
            health += amount;
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
            switch (gameObject.tag)
            {
                case "Player":
                    buildingConstructor = GameObject.Find("Player").GetComponent<BuildingConstructor>(); break;
                case "Enemy":
                    buildingConstructor = GameObject.Find("Enemy").GetComponent<BuildingConstructor>(); break;
            }
        }


    }
}
