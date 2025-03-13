using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEffect/Item07Effect")]
public class Item07Effect : ItemEffect
{
    public static event System.Action<float> OnItem07Effect; // 定义事件
    public float amount;
    public override void ApplyEffect()
    {
        OnItem07Effect?.Invoke(amount);
    }
}