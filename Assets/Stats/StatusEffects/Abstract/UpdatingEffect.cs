using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal abstract class UpdatingEffect : StatusEffect
{
    private protected override void Start()
    {
        base.Start();
        GameManager.FixedUpdateEvent += FixedUpdate;
    }

    internal override void Stop()
    {
        base.Stop();
        GameManager.FixedUpdateEvent -= FixedUpdate;
    }

    protected abstract void FixedUpdate();
}
