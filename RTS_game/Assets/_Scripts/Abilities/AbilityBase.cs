using RTS.Objects;
using UnityEngine;

namespace RTS.Ability
{
    [CreateAssetMenu(fileName = "New Ability")]
    public abstract class AbilityBase : ScriptableObject
    {
        [SerializeField] protected string abilityName = "Undefined name";
        [SerializeField] protected float cooldown = 0;
        [SerializeField] protected float duration = 0;
        public abstract void Setup(SelectableObject selectedObject);
        public abstract void Activate();
        public abstract void Deactivate();
        public float GetDuration() => duration;
        public float GetCooldown() => cooldown;
        public string GetAbilityName() => abilityName;
    }
}

