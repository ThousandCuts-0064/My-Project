using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class FlatStat : Stat
{
    public override float Neutral => 0;

    public FlatStat(float baseValue) : base(baseValue) { }
    private FlatStat() : base() { }

    public void ModFlat(FlatStat flatStat) => base.ModFlat(flatStat);
    public void ModMult(MultStat multStat) => base.ModMult(multStat);
}
