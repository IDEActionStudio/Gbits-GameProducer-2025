using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ItemEffect/Item06Effect")]
public class Item06Effect : ItemEffect
{
    public static event System.Action OnItem06Effect; // 定义事件
    public override void ApplyEffect()
    {
        OnItem06Effect?.Invoke();
    }
}