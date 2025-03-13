using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactive
{
    public ItemEffect itemEffect;
    protected override void MakeSomeReaction()
    {
        itemEffect.ApplyEffect();
        Destroy(gameObject);
    }
}
