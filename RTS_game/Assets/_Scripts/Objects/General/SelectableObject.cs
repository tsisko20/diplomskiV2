using RTS.Ability;
using RTS.InputManager;
using RTS.UI;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.Objects
{
    public abstract class SelectableObject : MonoBehaviour
    {
        public Material redMaterial;
        public Material greenMaterial;
        public Material neutralMaterial;
        protected float health;
        protected float armor;
        protected float attackDamage;
        protected float attackRange;
        protected float attackSpeed;
        protected float moveSpeed;
        protected AbilityHolder[] abilityHolders;
        public SpriteRenderer minimapIcon;
        private Image healthBar;

        private void Awake()
        {
            healthBar = transform.Find("StatsCanvas/HealthBackground/Health").GetComponent<Image>();
            Setup();
            abilityHolders = GetComponents<AbilityHolder>();
        }

        public void Select()
        {
            transform.Find("selectionSprite").gameObject.SetActive(true);
        }

        public void Deselect()
        {
            transform.Find("selectionSprite").gameObject.SetActive(false);
        }




        public abstract bool IsDead();

        public void SetTeamByHierarchy()
        {
            switch (transform.root.name)
            {
                case "Player":
                    tag = "Player";
                    break;
                case "Enemy":
                    tag = "Enemy";
                    break;
                default: tag = "Neutral"; break;
            }
            GameObject parentFolderRoot;
            parentFolderRoot = GameObject.Find(GetInstanceDestination());
            transform.SetParent(parentFolderRoot.transform);
            healthBar.color = GetTeamColor();
        }

        public void SetTeam(string team)
        {
            gameObject.tag = team;
            GameObject parentFolderRoot;
            parentFolderRoot = GameObject.Find(GetInstanceDestination());
            transform.SetParent(parentFolderRoot.transform);
            healthBar.color = GetTeamColor();
            minimapIcon.color = GetTeamColor();
        }

        protected abstract string GetInstanceDestination();

        protected Color GetTeamColor()
        {
            switch (gameObject.tag)
            {
                case "Player":
                    return TeamColors.Player;
                case "Enemy":
                    return TeamColors.Enemy;
                case "Neutral":
                    return TeamColors.Neutral;
            }
            return Color.white;
        }
        public abstract void SetColor(Color color);
        public abstract BasicObject GetBaseStats();
        public abstract string GetObjectName();
        public float GetCurrentHealth() => health;

        public void IncreaseCurrentHealth(float amount)
        {
            health = Mathf.Clamp(health + amount, 0, GetBaseStats().baseStats.health);
        }

        protected abstract void Setup();

        public virtual AbilityHolder[] GetAbilityHolders() => abilityHolders;

        private void OnMouseEnter()
        {
            if (gameObject.tag == "Enemy")
            {
                if (InputHandler.instance.IsPlayerSelected())
                    CursorManager.instance.SetActiveCursorType(CursorType.Attack);
                else
                    CursorManager.instance.SetActiveCursorType(CursorType.EnemyBasic);
            }
            if (gameObject.tag == "Player")
            {
                CursorManager.instance.SetActiveCursorType(CursorType.PlayerBasic);
            }
        }

        public abstract string GetTeam();

        private void OnMouseExit()
        {
            CursorManager.instance.SetActiveCursorType(CursorType.Basic);
        }
    }
}