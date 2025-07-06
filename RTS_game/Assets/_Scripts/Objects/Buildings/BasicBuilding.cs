using UnityEngine;

namespace RTS.Buildings
{
    [CreateAssetMenu(fileName = "Building", menuName = "New Building /Basic Building Template")]
    public partial class BasicBuilding : BasicObject
    {
        public enum BuildingType
        {
            ResourceStorage,
            Barracks,
            DefenseTower,
            Farm,
            CapturePoint,
            Sanctuary
        }

        [Space(15)]
        [Header("Building Setting")]
        [Space(5)]

        public BuildingType type;
        public string buildingName;



    }

}

