using UnityEngine;

namespace RTS.Units
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/Basic Unit Template")]
    public partial class BasicUnit : BasicObject
    {
        [Space(15)]
        [Header("Unit Settings")]
        [Space(5)]
        public UnitType unitType;
        public string unitName;




    }
}

