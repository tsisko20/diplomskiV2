using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RTS
{
    public class BasicStats : ScriptableObject
    {
        [System.Serializable]
        public class Base
        {
            public float cost, aggroRange, attackRange, attackSpeed, attackDamage, health, armor;

        }
    }
}
