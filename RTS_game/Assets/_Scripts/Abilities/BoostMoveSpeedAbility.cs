using RTS.Objects;
using RTS.Objects.Units;
using RTS.UI;
using UnityEngine;

namespace RTS.Ability
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Boost Move Speed Ability")]
    public class BoostMoveSpeedAbility : AbilityBase
    {
        [SerializeField] private float speedMultiplier;
        private float baseSpeed;
        private Unit unit;

        public override void Setup(SelectableObject selectedObject)
        {
            if (selectedObject is Unit)
            {
                unit = (Unit)selectedObject;
            }
        }
        public override void Activate()
        {
            baseSpeed = unit.navAgent.speed;
            unit.navAgent.speed = baseSpeed * speedMultiplier;
        }

        public override void Deactivate()
        {
            if (unit != null)
            {
                unit.navAgent.speed = baseSpeed;
            }
        }

    }
}

