using UnityEngine;

namespace RTS
{
    public class BasicStats : ScriptableObject
    {
        [System.Serializable]
        public class Base
        {
            public float goldCost, woodCost, aggroRange, attackRange, attackSpeed, attackDamage, health, armor;

        }
    }
}
