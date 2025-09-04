using RTS.Objects;
using UnityEngine;

namespace RTS.Ability
{
    public class AbilityHolder : MonoBehaviour
    {
        protected AbilityStateType abilityState;
        public AbilityBase abilityTemplate;
        public AbilityBase abilityInstance;
        [SerializeField] protected float activeDuration = 0;
        [SerializeField] protected float activeCooldown = 0;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private AbilityButtonUI buttonUI;

        private void Start()
        {
            Setup();
        }
        private void Update()
        {
            switch (abilityState)
            {
                case AbilityStateType.Active:
                    if (activeDuration < abilityInstance.GetDuration())
                    {
                        activeDuration += Time.deltaTime;
                        if (buttonUI)
                            buttonUI.UpdateActiveAnimation(activeDuration);
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
                        if (buttonUI)
                            buttonUI.UpdateCdAnimation(activeCooldown);
                    }
                    else
                    {
                        activeCooldown = 0;
                        abilityState = AbilityStateType.Ready;
                    }
                    break;
            }
        }

        public void Setup()
        {
            abilityState = AbilityStateType.Ready;
            SelectableObject caster = GetComponent<SelectableObject>();
            if (abilityTemplate != null && caster != null)
            {
                abilityInstance = Instantiate(abilityTemplate);
                abilityInstance.Setup(caster);
            }
        }
        public GameObject GetButtonPrefab() => buttonPrefab;

        public void ActivateAbility()
        {
            abilityInstance.Activate();
            abilityState = AbilityStateType.Active;
        }

        public AbilityStateType GetAbilityState() => abilityState;

        public void SetButtonUIComponent(AbilityButtonUI buttonUIComponent)
        {
            buttonUI = buttonUIComponent;
            buttonUI.Setup(abilityInstance.GetCooldown(), abilityInstance.GetDuration());
        }
    }

}
