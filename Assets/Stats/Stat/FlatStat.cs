using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class FlatStat : Stat
{
    public const float NEUTRAL = 0;
    public override StatType Type => StatType.Flat;
    public override float Neutral => NEUTRAL;

    public FlatStat(float baseValue) : base(baseValue) { }
    private FlatStat() : base() { }

    public void ModFlat(FlatStat flatStat) => base.ModFlat(flatStat);
    public void ModMult(MultStat multStat) => base.ModMult(multStat);
}
