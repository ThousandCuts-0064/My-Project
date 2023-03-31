using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal class StatAffecting : TemporaryStatusEffect
{
    private Stat _targetStat;
    public string StatName { get; set; }
    public ModType ModType { get; set; }

    public StatAffecting(float duration) : base(duration) { }

    internal override bool TryStart(Stats stats)
    {
        if (!stats.TryGetStat(StatName, out _targetStat))
            return false;

        base.TryStart(stats);
        switch (_targetStat)
        {
            case FlatStat flatStat:
                switch (ModType)
                {
                    case ModType.Flat: flatStat.ModFlat((FlatStat)Stat); break;
                    case ModType.Mult: flatStat.ModMult((MultStat)Stat); break;
                    default: throw new InvalidOperationException();
                }
                break;

            case MultStat multStat:
                switch (ModType)
                {
                    case ModType.Flat: multStat.ModFlat((MultStat)Stat); break;
                    case ModType.Mult: multStat.ModMult((MultStat)Stat); break;
                    default: throw new InvalidOperationException();
                }
                break;
        }
        return true;
    }

    internal override void Stop()
    {
        base.Stop();
        Stat.RemoveFromOthers();
    }
}
