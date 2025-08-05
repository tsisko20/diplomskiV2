using UnityEngine;

namespace RTS
{
    public interface IAttackable : ITargetable
    {
        void TakeDamage(float amount);
        Team GetTeam();
        bool IsDead();
    }
}