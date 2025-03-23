using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Item10Effect")]
public class Item10Effect : ItemEffect
{
    public static event System.Action OnItem10Effect; // 定义事件

    public override void ApplyEffect()
    {
        OnItem10Effect?.Invoke();
    }
}