using RTS.Ability;
using RTS.Objects;
using Unity.VisualScripting;
using UnityEngine;

namespace RTS.Ability
{
    public class AbilityHolder : MonoBehaviour
    {
        protected AbilityStateType abilityState;
        [SerializeField] private AbilityBase abilityTemplate;
        private AbilityBase abilityInstance;
        [SerializeField] protected float activeDuration = 0;
        [SerializeField] protected float activeCooldown = 0;
        [SerializeField] private GameObject buttonPrefab;

        private void Start()
        {
            abilityState = AbilityStateType.Ready;
            SelectableObject caster = GetComponent<SelectableObject>();
            if (abilityTemplate != null && caster != null)
            {
                abilityInstance = Instantiate(abilityTemplate);
                abilityInstance.SetCaster(caster);
            }
        }
        private void Update()
        {
            switch (abilityState)
            {
                case AbilityStateType.Active:
                    if (activeDuration < abilityInstance.GetDuration())
                    {
                        activeDuration += Time.deltaTime;
                    }
                    else
                    {
                        abilityInstance.Deactivate();
                        activeDuration = 0;
                        abilityState = AbilityStateType.Cooldown;
                    }
                    break;
                case AbilityStateType.Cooldown:
                    if (activeCooldown < abilityInstance.GetCooldown())
                    {
                        activeCooldown += Time.deltaTime;
                    }
                    else
                    {
                        activeCooldown = 0;
                        abilityState = AbilityStateType.Ready;
                    }
                    break;
            }
        }
        public GameObject GetButtonPrefab() => buttonPrefab;

        public void ActivateAbility()
        {
            abilityInstance.Activate();
            abilityState = AbilityStateType.Active;
        }

        public AbilityStateType GetAbilityState() => abilityState;

    }

}
