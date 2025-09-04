using UnityEngine;
namespace RTS.Units
{
    public class UnitHandler : MonoBehaviour
    {
        public static UnitHandler instance;
        [SerializeField]
        private BasicUnit worker, warrior, archer;
        public LayerMask playerLayer, enemyLayer;

        private void Start()
        {
            instance = this;
        }
        public BasicStats.Base GetBasicUnitStats(string type)
        {
            BasicUnit unit;
            switch (type)
            {
                case "worker": unit = worker; break;
                case "warrior": unit = warrior; break;
                case "archer": unit = archer; break;
                default:
                    
                    return null;
            }
            return unit.baseStats;
        }

    }
}
