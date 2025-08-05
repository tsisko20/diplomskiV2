using UnityEngine;
namespace RTS.Units
{
    public class UnitStatTypes : ScriptableObject
    {
        [System.Serializable]
        public class Base
        {
            public float cost, aggroRange, attackRange, attackSpeed, attackDamage, health, armor;
        }
    }
}

