using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal abstract class UpdatingStatusEffect<T> : GenericStatusEffect<T> where T : Stats
{
    protected UpdatingStatusEffect(T stats) : base(stats) { }

    internal override void Start() => GameManager.FixedUpdateEvent += FixedUpdate;

    internal override void Stop() => GameManager.FixedUpdateEvent -= FixedUpdate;

    protected abstract void FixedUpdate();
}
