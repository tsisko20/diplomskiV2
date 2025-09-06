using RTS.Ability;
using RTS.Buildings;
using RTS.InputManager;
using RTS.UI;
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
        [SerializeField] public BasicBuilding buildingStats;
        public MeshRenderer meshRenderer;
        [SerializeField] private BoxCollider boxCollider;
        private TeamResourceStorages teamResourceStorages;
        public BuildingState state;
        public bool constructionFinished = false;
        public NavMeshObstacle navMeshObstacle;
        public Recruiter recruiter;

        protected override void Setup()
        {

            SetTeamByHierarchy();
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
                if (tag == "Player")
                    SetColor(Color.lightGreen);
                if (tag == "Enemy")
                    SetColor(Color.softRed);
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

                    SetColor(Color.white);
                    if (buildingStats.type == BasicBuilding.BuildingType.ResourceStorage)
                    {
                        teamResourceStorages = ResourceHandler.GetTeamStorage(tag);
                        teamResourceStorages.AddResStorage(gameObject);
                    }
                    foreach (var abilityHolder in abilityHolders)
                    {
                        abilityHolder.Setup();
                    }
                    if (InputHandler.GetSelectableObjects().Contains(transform))
                    {
                        SelectedObjectUI.UpdateUI(InputHandler.GetSelectableObjects());
                    }
                    state = BuildingState.Finished;
                }
            }
        }

        protected override string GetInstanceDestination()
        {
            string parentFolder = buildingStats.buildingName + "s";
            string root = gameObject.tag;
            return $"{gameObject.tag}/Buildings/{parentFolder}";
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

        protected void Die()
        {
            if (InputHandler.GetSelectableObjects().Remove(gameObject.GetComponent<Transform>()))
            {
                SelectedObjectUI.UpdateUI(InputHandler.GetSelectableObjects());
            }
            Collider col = GetComponent<Collider>();
            if (col) col.enabled = false;
            WinCondition.CallTestWinCondition();
            if (buildingStats.type == BasicBuilding.BuildingType.ResourceStorage)
            {
                if (teamResourceStorages != null)
                    teamResourceStorages.allResourceStorages.RemoveAll(item => item == null);
            }
            Destroy(gameObject);
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