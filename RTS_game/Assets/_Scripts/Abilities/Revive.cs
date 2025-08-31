using RTS.Objects;
using RTS.Objects.Units;
using RTS.UI;
using UnityEngine;

namespace RTS.Ability
{
    [CreateAssetMenu(fileName = "Ability", menuName = "Revive Ability")]
    public class ReviveAbility : AbilityBase
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
                if (unit != null && unit.IsDead() && caster.tag == unit.tag)
                {
                    Revive(unit);
                    unit.stateMachine.ChangeState(unit.stateMachine.idleState);
                }
            }
        }

        public override void Deactivate()
        {

        }

        public void Revive(Unit unit)
        {
            unit.navAgent.enabled = true;
            unit.movingRange.gameObject.SetActive(false);
            unit.animator.ChangeAnimation(UnitStates.Dead);
            unit.Heal(10);
        }

    }
}

