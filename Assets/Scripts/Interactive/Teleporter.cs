using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Teleporter : Interactive
{
    public UnityEvent onTeleport;
    protected override void MakeSomeReaction()
    {
        base.MakeSomeReaction();
        onTeleport.Invoke();
    }
}
