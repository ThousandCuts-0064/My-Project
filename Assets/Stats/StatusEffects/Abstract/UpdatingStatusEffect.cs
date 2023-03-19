using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal abstract class UpdatingStatusEffect : StatusEffect
{
    internal override void Start(Stats stats) => GameManager.FixedUpdateEvent += FixedUpdate;

    internal override void Stop() => GameManager.FixedUpdateEvent -= FixedUpdate;

    protected abstract void FixedUpdate();
}
