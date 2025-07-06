using UnityEngine;

namespace RTS
{
    public interface IAttackable
    {
        void TakeDamage(float amount);
        Team GetTeam();
        Transform GetTransform();
        bool IsDead();
    }
}