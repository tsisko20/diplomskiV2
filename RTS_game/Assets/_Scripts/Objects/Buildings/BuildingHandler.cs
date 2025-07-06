using UnityEngine;

namespace RTS.Buildings
{
    public class BuildingHandler : MonoBehaviour
    {
        public static BuildingHandler instance;
        [SerializeField]
        private BasicBuilding resourceStorage, farm, barracks, defenseTower, sanctuary;

        private void Start()
        {
            instance = this;
        }
        public BasicStats.Base GetBasicUnitStats(string type)
        {
            BasicBuilding building;
            switch (type)
            {
                case "resourceStorage": building = resourceStorage; break;
                case "barrack": building = barracks; break;
                case "farm": building = farm; break;
                case "defenseTower": building = defenseTower; break;
                case "sanctuary": building = sanctuary; break;
                default:
                    Debug.Log($"Building type: {type} could not be found or does not exist!");
                    return null;
            }
            return building.baseStats;
        }
    }
}

