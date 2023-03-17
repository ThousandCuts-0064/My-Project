using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal abstract class TemporaryStatusEffect<T> : UpdatingStatusEffect<T> where T : IInternalStats
{
    [field: SerializeField] public float Duration { get; private set; }
    internal override bool IsTemporary => true;

    internal event Action Finished;

    protected TemporaryStatusEffect(T stats, float duration) : base(stats) => Duration = duration;

    protected override void FixedUpdate()
    {
        Duration -= Time.fixedDeltaTime;
        if (Duration <= 0)
        {
            Finished?.Invoke();
            Stop();
        }
    }
}
