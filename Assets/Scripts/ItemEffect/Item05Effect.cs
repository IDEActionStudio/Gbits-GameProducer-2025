using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEffect/Item05Effect")]
public class Item05Effect : ItemEffect
{
    public static event System.Action OnItem05Effect; // 定义事件
    public override void ApplyEffect()
    {
        OnItem05Effect?.Invoke();
    }
}