using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class MultStat : Stat
{
    public const int NEUTRAL = 1;
    public const int LOWER_BOUND = 0;
    public override StatType Type => StatType.Mult;
    public override float Neutral => NEUTRAL;
    public override float Value => Math.Max(LOWER_BOUND, base.Value);

    public MultStat(float baseValue) : base(baseValue)
    {
        if (baseValue < LOWER_BOUND)
            throw new ArgumentOutOfRangeException(nameof(baseValue), baseValue, $"Cannot be less than {LOWER_BOUND}");
    }
    private MultStat() : base() { }

    public void ModFlat(MultStat multStat) => base.ModFlat(multStat);
    public void ModMult(MultStat multStat) => base.ModMult(multStat);
}
