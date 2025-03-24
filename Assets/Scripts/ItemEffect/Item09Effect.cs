using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/Item09Effect")]
public class Item09Effect : ItemEffect
{
    public static event System.Action OnItem09Effect; // 定义事件

    public override void ApplyEffect()
    {
        OnItem09Effect?.Invoke();
    }
}