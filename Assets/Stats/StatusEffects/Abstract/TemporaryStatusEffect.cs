using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal abstract class TemporaryStatusEffect<T> : GenericStatusEffect<T> where T : Stats
{
    [field: SerializeField] public float Duration { get; private set; }
    public override bool IsTemporary => true;

    public event Action Finished;

    protected TemporaryStatusEffect(T stats, float duration) : base(stats) => Duration = duration;

    protected virtual void FixedUpdate()
    {
        Duration -= Time.fixedDeltaTime;
        if (Duration <= 0)
        {
            Finished?.Invoke();
            Stop();
        }
    }
}
