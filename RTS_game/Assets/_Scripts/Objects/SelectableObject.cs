using NUnit.Framework;
using RTS.Ability;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static RTS.BasicObject;

namespace RTS.Objects
{
    public abstract class SelectableObject : MonoBehaviour
    {
        public Material redMaterial;
        protected Team team;
        public Material blueMaterial;
        public Material neutralMaterial;
        public SkinnedMeshRenderer rend;
        protected float health;
        protected float armor;
        protected float attackDamage;
        protected float attackRange;
        protected float attackSpeed;
        protected float moveSpeed;
        private AbilityHolder[] abilityHolders;

        private void Start()
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
        protected void SetTeam()
        {
            switch (transform.root.name)
            {
                case "Player": team = Team.Player; break;
                case "Enemy": team = Team.Enemy; break;
                default: team = Team.Neutral; break;
            }
        }
        protected void SetMaterial()
        {
            switch (team)
            {
                case Team.Player:
                    rend.material = blueMaterial;
                    break;
                case Team.Enemy:
                    rend.material = redMaterial;
                    break;
                case Team.Neutral:
                    rend.material = neutralMaterial;
                    break;
            }
        }

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

        protected abstract void Setup();

        public AbilityHolder[] GetAbilityHolders() => abilityHolders;

    }
}