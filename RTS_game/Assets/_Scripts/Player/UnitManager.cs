using RTS.InputManager;
using RTS.Units;
using UnityEngine;
namespace RTS.Player
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager instance;
        public Transform playerUnits;
        public Transform enemyUnits;
        public Transform playerBuildings;
        public Transform enemyBuildings;


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
                    Debug.Log($"Unit type: {type} could not be found or does not exist!");
                    return null;
            }
            return unit.baseStats;
        }
    }
}

