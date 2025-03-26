using UnityEngine;
[CreateAssetMenu(menuName = "ItemEffect/Item01Effect")]
public class Item01Effect : ItemEffect
{
    public static event System.Action OnItem01Effect; // 定义事件
    public override void ApplyEffect()
    {
        OnItem01Effect?.Invoke();
    }
}