using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static RTS.BasicObject;

namespace RTS.InputManager
{
    public abstract class SelectableObject : MonoBehaviour
    {
        public Material redMaterial;
        protected Team team;
        public Material blueMaterial;
        public Material neutralMaterial;
        public SkinnedMeshRenderer rend;
        protected float currentHealth;

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

        public abstract BasicObject GetBaseStats();
        public abstract string GetObjectName();
        public float GetCurrentHealth() => currentHealth;
    }
}