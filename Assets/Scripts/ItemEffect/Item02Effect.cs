using UnityEngine;
[CreateAssetMenu(menuName = "ItemEffect/Item02Effect")]
public class Item02Effect : ItemEffect
{
    public static event System.Action OnItem02Effect; // 定义事件
    public override void ApplyEffect()
    {
        OnItem02Effect?.Invoke();
    }
}