using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Item08Effect")]
public class Item08Effect : ItemEffect
{
    public static event System.Action OnItem08Effect; // 定义事件

    public override void ApplyEffect()
    {
        OnItem08Effect?.Invoke();
    }
}