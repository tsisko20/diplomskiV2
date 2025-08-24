using UnityEngine;

namespace RTS
{
    public interface IAttackable : ITargetable
    {
        void TakeDamage(float amount);
        bool IsDead();
        string GetTeam();
    }
}