using UnityEngine;

namespace EnemySystem.Core.Interfaces
{
    // IDamageable.cs
    public interface IDamageable 
    {
        void TakeDamage(float damage);
        void TakeDamage(float damage, Vector3 hitPoint); // 支持击中点
        float CurrentHealth { get; }
    }
}