using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal class StatAffecting : TemporaryStatusEffect
{
    private Stat _stat;
    public string StatName { get; set; }
    public ModType ModType { get; set; }

    public StatAffecting(float duration) : base(duration) { }

    internal override bool TryStart(Stats stats)
    {
        if (!stats.TryGetStat(StatName, out _stat))
            return false;

        base.TryStart(stats);
        switch (ModType)
        {
            case ModType.None: throw new InvalidOperationException();
            case ModType.Flat: _stat.ModFlat(_stat); break;
            case ModType.Mult: _stat.ModMult(_stat); break;
            default: throw new ArgumentOutOfRangeException(nameof(ModType));
        }
        return true;
    }

    internal override void Stop()
    {
        base.Stop();
        Stat.RemoveFromOthers();
    }
}
