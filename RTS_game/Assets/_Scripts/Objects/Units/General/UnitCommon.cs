using RTS.InputManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace RTS.Units
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class UnitCommon : MonoBehaviour
    {
        protected NavMeshAgent navAgent;
        public BasicStats.Base baseStats;
        public float currentHealth;



        protected virtual void Start()
        {
            navAgent = GetComponent<NavMeshAgent>();
            currentHealth = baseStats.health;
        }

        protected virtual void Die()
        {
            InputManager.InputHandler.GetSelectableObjects().Remove(gameObject.GetComponent<Transform>());
            Destroy(gameObject);
        }
        public void TakeDamage(float damage)
        {
            float totalDamage = damage - (damage * (baseStats.armor / 100));
            currentHealth -= totalDamage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        public void MoveUnit(Vector3 destination)
        {
            navAgent.SetDestination(destination);
        }
    }
}
