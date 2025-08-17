using UnityEngine;

namespace RTS.Objects.Units
{
    public class CombatBehaviour : MonoBehaviour
    {
        private Unit unit;
        [SerializeField] private IAttackable attackTarget;
        private float attackCooldown;
        void Start()
        {
            unit = GetComponent<Unit>();
        }

        void Update()
        {
            if (unit.IsDead() == false)
            {
                if (unit.target == null)
                {
                    if (attackTarget != null)
                        attackTarget = null;
                    if (unit.state == UnitStates.Idle)
                        SearchForTarget();
                }
                else
                {
                    if (unit.target is IAttackable)
                    {
                        attackTarget = (IAttackable)unit.target;
                        if (attackTarget.IsDead())
                        {
                            unit.target = null;
                            attackTarget = null;
                            unit.ChangeState(UnitStates.Idle);
                        }
                        else
                        {
                            Collider targetCollider = attackTarget.GetTransform().GetComponent<Collider>();
                            Vector3 closestPoint = targetCollider.ClosestPoint(unit.transform.position);
                            float distance = Vector3.Distance(unit.transform.position, closestPoint);

                            if (distance <= unit.unitStats.baseStats.attackRange)
                            {
                                if (unit.state == UnitStates.Walking)
                                    unit.StopMoving();
                                AttackTarget();
                            }
                            else
                            {
                                unit.MoveTo(attackTarget.GetTransform().position);
                            }
                        }

                        if (unit.state == UnitStates.Attacking)
                        {
                            attackCooldown -= Time.deltaTime;
                        }
                    }
                }
            }
        }

        public void SetAggro(IAttackable _target)
        {
            unit.target = _target;
            attackTarget = _target;
        }

        void SearchForTarget()
        {
            var hits = Physics.OverlapSphere(transform.position, unit.unitStats.baseStats.aggroRange);
            foreach (var hit in hits)
            {
                var potentialTarget = hit.GetComponent<IAttackable>();
                if (potentialTarget != null && potentialTarget.GetTeam() != unit.GetTeam() && potentialTarget.GetTeam() != Team.Neutral && potentialTarget.IsDead() == false)
                {
                    Debug.Log(potentialTarget.GetTeam());
                    SetAggro(potentialTarget);
                    break;
                }

            }

        }


        private void AttackTarget()
        {
            if (unit.state != UnitStates.Attacking)
                unit.ChangeState(UnitStates.Attacking);
            Vector3 directionToTarget = attackTarget.GetTransform().position - transform.position;
            directionToTarget.y = 0f;
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            if (attackCooldown <= 0)
            {
                attackTarget.TakeDamage(unit.unitStats.baseStats.attackDamage);
                attackCooldown = unit.unitStats.baseStats.attackSpeed;
            }
        }



    }

}