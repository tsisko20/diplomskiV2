using NUnit.Framework;
using RTS.Ability;
using RTS.InputManager;
using RTS.UI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static RTS.BasicObject;

namespace RTS.Objects
{
    public abstract class SelectableObject : MonoBehaviour
    {
        public Material redMaterial;
        public Team team;
        public Material greenMaterial;
        public Material neutralMaterial;
        protected float health;
        protected float armor;
        protected float attackDamage;
        protected float attackRange;
        protected float attackSpeed;
        protected float moveSpeed;
        private AbilityHolder[] abilityHolders;

        private void Awake()
        {
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

        public Team GetTeam() => team;
        protected void SetTeamByHierarchy()
        {
            switch (transform.root.name)
            {
                case "Player":
                    team = Team.Player;
                    break;
                case "Enemy":
                    team = Team.Enemy;
                    break;
                default:
                    team = Team.Neutral;
                    break;
            }
        }

        public void SetTeam(Team _team)
        {
            team = _team;
            SetColor(GetTeamColor());
        }
        protected Color GetTeamColor()
        {
            switch (team)
            {
                case Team.Player:
                    return TeamColors.Player;
                case Team.Enemy:
                    return TeamColors.Enemy;
                case Team.Neutral:
                    return TeamColors.Neutral;
            }
            return Color.white;
        }

        public abstract void SetColor(Color color);

        public void TakeDamage(float damage)
        {

            float totalDamage = damage - (damage * (armor / 100));
            health -= totalDamage;
            if (health <= 0)
            {
                Die();
            }
        }

        protected abstract void Die();

        public abstract BasicObject GetBaseStats();
        public abstract string GetObjectName();
        public float GetCurrentHealth() => health;

        public void IncreaseCurrentHealth(float amount)
        {
            health = Mathf.Clamp(health + amount, 0, GetBaseStats().baseStats.health);
        }

        protected abstract void Setup();

        public AbilityHolder[] GetAbilityHolders() => abilityHolders;

        private void OnMouseEnter()
        {
            if (team == Team.Enemy)
            {
                if (InputHandler.instance.IsPlayerSelected())
                    CursorManager.instance.SetActiveCursorType(CursorType.Attack);
                else
                    CursorManager.instance.SetActiveCursorType(CursorType.EnemyBasic);
            }
            if (team == Team.Player)
            {
                CursorManager.instance.SetActiveCursorType(CursorType.PlayerBasic);
            }
        }

        private void OnMouseExit()
        {
            CursorManager.instance.SetActiveCursorType(CursorType.Basic);
        }
    }
}