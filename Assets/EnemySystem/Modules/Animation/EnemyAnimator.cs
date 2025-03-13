using UnityEngine;

namespace EnemySystem.Modules.Animation
{
    /// <summary>
    /// 动画控制器 - 管理状态机和动画混合
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int HitHash = Animator.StringToHash("Hit");
        
        private Animator _animator;

        private void Awake() => _animator = GetComponent<Animator>();

        /// <summary>
        /// 更新移动速度参数
        /// </summary>
        public void UpdateMovementSpeed(float speed)
        {
            _animator.SetFloat(SpeedHash, speed);
        }

        /// <summary>
        /// 触发攻击动画
        /// </summary>
        public void TriggerAttack()
        {
            _animator.SetTrigger(AttackHash);
        }

        /// <summary>
        /// 播放受击反应动画
        /// </summary>
        public void PlayHitReaction()
        {
            _animator.SetTrigger(HitHash);
        }
    }
}