using RTS.Objects;
using RTS.Objects.Units;
using UnityEngine;

namespace RTS.Ability
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Boost Move Speed Ability")]
    public class BoostMoveSpeedAbility : AbilityBase
    {
        [SerializeField] private float speedMultiplier;
        private float baseSpeed;
        private Unit unit;
        public override void Activate()
        {
            if (caster is Unit)
            {
                unit = (Unit)caster;
                baseSpeed = unit.navAgent.speed;
                unit.navAgent.speed = baseSpeed * speedMultiplier;
            }
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

