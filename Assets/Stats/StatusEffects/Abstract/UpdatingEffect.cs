using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal abstract class UpdatingEffect : StatusEffect
{
    internal override bool TryStart(Stats stats)
    {
        GameManager.FixedUpdateEvent += FixedUpdate;
        return true;
    }

    internal override void Stop() => GameManager.FixedUpdateEvent -= FixedUpdate;

    protected abstract void FixedUpdate();
}
