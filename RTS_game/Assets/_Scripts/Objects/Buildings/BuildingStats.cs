using UnityEngine;

namespace RTS.Buildings
{
    public class BuildingStats : ScriptableObject
    {
        [System.Serializable]
        public class Base
        {
            public float cost, health, armor, attackDamage, attackSpeed;
        }
    }
}
