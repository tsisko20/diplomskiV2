using RTS;
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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Setup()
        {

            SetTeamByHierarchy();
            meshRenderer.material.color = GetTeamColor();
            teamResourceStorages = transform.root.GetComponent<TeamResourceStorages>();
            navMeshObstacle = transform.GetComponent<NavMeshObstacle>();
        }

        private void Start()
        {
            if (state == BuildingState.Init)
            {
                health = 5;
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
                }
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