using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Objects.Units
{
    public class MovingRange : MonoBehaviour
    {
        [SerializeField] private List<Unit> unitsInMovingRange;
        private void Start()
        {
            unitsInMovingRange = new List<Unit>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Unit otherUnit))
            {
                if (unitsInMovingRange.Contains(otherUnit) == false)
                {
                    unitsInMovingRange.Add(otherUnit);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Unit otherUnit))
            {
                if (unitsInMovingRange.Contains(otherUnit))
                {
                    unitsInMovingRange.Remove(otherUnit);
                }
            }
        }

        public List<Unit> GetUnitsInMovingRange()
        {
            return unitsInMovingRange;
        }
    }
}

