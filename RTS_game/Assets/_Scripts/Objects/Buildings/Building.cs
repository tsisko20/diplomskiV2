using RTS;
using RTS.Ability;
using RTS.Buildings;
using RTS.InputManager;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.Objects.Buildings
{

    public enum BuildingState
    {
        Init,
        Constructing,
        Finished
    }

    [RequireComponent(typeof(NavMeshObstacle))]
    public class Building : SelectableObject, IAttackable
    {
        [SerializeField] private BasicBuilding buildingStats;
        public MeshRenderer meshRenderer;
        [SerializeField] private BoxCollider boxCollider;
        private TeamResourceStorages teamResourceStorages;
        public BuildingState state;
        public bool constructionFinished = false;
        public NavMeshObstacle navMeshObstacle;
        public Recruiter recruiter;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Setup()
        {

            SetTeamByHierarchy();
            meshRenderer.material.color = GetTeamColor();
            minimapIcon.color = GetTeamColor();
            teamResourceStorages = transform.root.GetComponent<TeamResourceStorages>();
            navMeshObstacle = transform.GetComponent<NavMeshObstacle>();
            recruiter = gameObject.GetComponent<Recruiter>();
        }

        private void Start()
        {
            if (state == BuildingState.Init)
            {
                health = 5;
                SetColor(Color.lightGreen);
            }
            else
            {
                health = buildingStats.baseStats.health;
            }
        }

        public void Update()
        {
            if (state == BuildingState.Init)
            {
                if (health == buildingStats.baseStats.health)
                {
                    state = BuildingState.Finished;
                    SetColor(GetTeamColor());
                }
            }
        }

        public override AbilityHolder[] GetAbilityHolders()
        {
            if (state == BuildingState.Finished)
            {
                return abilityHolders;
            }
            else
            {
                return new AbilityHolder[0];
            }
        }

        public override string GetTeam() => gameObject.tag;

        public void TakeDamage(float damage)
        {

            float totalDamage = damage - (damage * (armor / 100));
            health -= totalDamage;
            if (health <= 0)
            {
                Die();
            }
        }

        public override string GetObjectName()
        {
            return buildingStats.buildingName;
        }

        public override BasicObject GetBaseStats()
        {
            return buildingStats;
        }


        public Transform GetTransform() => transform;

        public override bool IsDead() => health <= 0;

        protected override void Die()
        {
            RTS.InputManager.InputHandler.instance.GetSelectableObjects().Remove(gameObject.GetComponent<Transform>());
            Collider col = GetComponent<Collider>();
            if (col) col.enabled = false;
            Destroy(gameObject);
            if (buildingStats.type == BasicBuilding.BuildingType.Farm)
            {
                teamResourceStorages.allResourceStorages.RemoveAll(item => item == null);
            }
        }

        public Color GetColor()
        {
            return meshRenderer.material.color;
        }

        public override void SetColor(Color color)
        {
            meshRenderer.material.color = color;
        }

        public Vector3 CenterPoint()
        {
            return transform.TransformPoint(boxCollider.center);
        }

        public Vector3 ColliderWorldSize()
        {
            var size = boxCollider.size;
            size.Scale(transform.lossyScale);
            return size;
        }

    }
}