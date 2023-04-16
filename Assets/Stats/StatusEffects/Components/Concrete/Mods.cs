using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected class _FlatModFlat : ModFlat<FlatStat>
    {
        internal _FlatModFlat(FlatStatType flatStatType, float modBase) : base(flatStatType) => Stat = new(modBase);

        internal override void Start() => TargetStat.ModFlat(Stat);
    }



    private protected class _MultModFlat : ModFlat<MultStat>
    {
        internal _MultModFlat(FlatStatType flatStatType, float modBase) : base(flatStatType) => Stat = new(modBase);

        internal override void Start() => TargetStat.ModMult(Stat);
    }



    private protected class _FlatModMult : ModMult<MultStat>
    {
        internal _FlatModMult(MultStatType multStatType, float modBase) : base(multStatType) => Stat = new(modBase);

        internal override void Start() => TargetStat.ModFlat(Stat);
    }



    private protected class _MultModMult : ModMult<MultStat>
    {
        internal _MultModMult(MultStatType multStatType, float modBase) : base(multStatType) => Stat = new(modBase);

        internal override void Start() => TargetStat.ModMult(Stat);
    }
}
