using RTS.Objects;
using RTS.Objects.Units;
using RTS.UI;
using UnityEngine;

namespace RTS.Ability
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Revive Ability")]
    public class HealAbility : AbilityBase
    {
        [SerializeField] private float revivedHealth;
        private Unit caster;
        private Unit deadUnit;
        private float reviveRadius = 5f;

        public override void Setup(SelectableObject selectedObject)
        {
            if (selectedObject is Unit)
            {
                caster = (Unit)selectedObject;
            }
        }
        public override void Activate()
        {
            Collider[] colliders = Physics.OverlapSphere(caster.transform.position, reviveRadius);
            foreach (var col in colliders)
            {
                Unit unit = col.GetComponent<Unit>();
                if (unit != null && caster.tag == unit.tag)
                {
                    unit.Heal(10);

                }
            }
        }

        public override void Deactivate()
        {

        }

    }
}

