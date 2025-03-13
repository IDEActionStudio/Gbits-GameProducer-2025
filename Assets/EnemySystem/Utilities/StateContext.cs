using UnityEngine;

/// <summary>
/// 状态间共享上下文数据
/// </summary>
public class StateContext
{
    public Vector3 LastKnownPlayerPosition { get; set; }
    public float LastAttackTime { get; set; }
    public int CurrentAttackPhase { get; set; }
    
    public void ResetCombat()
    {
        LastAttackTime = Time.time;
        CurrentAttackPhase = 0;
    }
}