using UnityEngine;

namespace RTS.Objects.Units
{
    public class CombatBehaviour : MonoBehaviour
    {
        private Unit unit;
        [SerializeField] private IAttackable target;
        private float attackCooldown;
        private bool isAttacking;
        public bool isAgressive = true;
        void Start()
        {
            unit = GetComponent<Unit>();
        }

        void Update()
        {
            if (unit.IsDead() == false && isAgressive)
            {
                if (target == null)
                {
                    SearchForTarget();
                }
                else
                {
                    if (target.IsDead())
                    {
                        target = null;
                        unit.animator.PlayAttackAnimation(false);
                    }

                    else
                        MoveAndAttack();
                }

                if (isAttacking)
                {
                    attackCooldown -= Time.deltaTime;
                }
            }
            else
            {
                unit.animator.PlayAttackAnimation(false);
            }
        }

        public void SetAggressive(bool setAggressive)
        {
            isAgressive = setAggressive;
            if (isAgressive == false)
            {
                SetAggro(null);
            }
        }

        public void SetAggro(IAttackable _target)
        {
            target = _target;
        }

        void SearchForTarget()
        {
            var hits = Physics.OverlapSphere(transform.position, unit.unitStats.baseStats.aggroRange);
            foreach (var hit in hits)
            {
                var potentialTarget = hit.GetComponent<IAttackable>();
                if (potentialTarget != null && potentialTarget.GetTeam() != unit.GetTeam() && unit.GetTeam() != Team.Neutral && potentialTarget.IsDead() == false)
                {


                    SetAggro(potentialTarget);
                    break;
                }

            }

        }

        void MoveAndAttack()
        {
            Collider targetCollider = target.GetTransform().GetComponent<Collider>();
            Vector3 closestPoint = targetCollider.ClosestPoint(unit.transform.position);
            float distance = Vector3.Distance(unit.transform.position, closestPoint);

            if (distance <= unit.unitStats.baseStats.attackRange)
            {
                unit.MoveUnit(transform.position);
                isAttacking = true;
                unit.animator.PlayAttackAnimation(true);
                Vector3 directionToTarget = target.GetTransform().position - transform.position;
                directionToTarget.y = 0f;
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                }
                if (attackCooldown <= 0)
                {
                    target.TakeDamage(unit.unitStats.baseStats.attackDamage);
                    attackCooldown = unit.unitStats.baseStats.attackSpeed;
                }
            }
            else
            {
                unit.animator.PlayAttackAnimation(false);
                isAttacking = false;
                unit.MoveUnit(target.GetTransform().position);
            }
        }
    }

}
