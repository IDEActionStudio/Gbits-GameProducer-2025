using EnemySystem.Core.AI;
using UnityEngine;
using UnityEngine.Events;

namespace EnemySystem.Utilities.EventSystem
{
    /// <summary>
    /// 类型安全事件系统
    /// </summary>
    public static class EventSystem
    {
        public class EnemyEvent : UnityEvent<EnemyBrain> {}
        public class DamageEvent : UnityEvent<GameObject, int> {}

        public static readonly EnemyEvent OnEnemySpawned = new EnemyEvent();
        public static readonly EnemyEvent OnEnemyDeath = new EnemyEvent();
        public static readonly DamageEvent OnDamageDealt = new DamageEvent();
    }
}