using RTS.Ability;
using RTS.Objects;
using UnityEngine;

public class EnemyContext : MonoBehaviour
{
    public TeamResourceStorages enemyStorage;
    public Transform workersParent;
    public Transform resourceStoragesParent;
    public Transform resourceStorageConstructLocation;
    public Transform barracksParent;
    public Transform barracksConstructLocation;
    public Transform archersParent;
    public float woodRequired;
    public float goldRequired;
    void Start()
    {
        enemyStorage = ResourceHandler.GetTeamStorage("Enemy");
    }

    public AbilityHolder FindAbility(string abilityName, SelectableObject caster)
    {
        AbilityHolder[] allAbilities = caster.GetAbilityHolders();
        foreach (AbilityHolder ability in allAbilities)
        {
            if (ability.abilityTemplate.GetAbilityName() == abilityName)
                return ability;
        }
        return null;
    }
}
