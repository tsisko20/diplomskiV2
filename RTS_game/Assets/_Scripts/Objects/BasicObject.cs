using UnityEngine;
using UnityEngine.UI;

namespace RTS
{
    public class BasicObject : ScriptableObject
    {
        protected enum ObjectType
        {
            Unit,
            Building,
            Tree,
            Mine
        }

        private ObjectType objectType;
        [Space(15)]
        [Header("Basic Stats")]
        [Space(5)]
        public BasicStats.Base baseStats;
        [SerializeField] private Sprite icon;
        public Sprite Icon
        {
            get { return icon; }
        }
    }

}

