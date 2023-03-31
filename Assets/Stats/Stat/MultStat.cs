using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class MultStat : Stat
{
    public MultStat(float baseValue) : base(baseValue) { }
    private MultStat() : base() { }

    public void ModFlat(MultStat multStat) => base.ModFlat(multStat);
    public void ModMult(MultStat multStat) => base.ModMult(multStat);
}
