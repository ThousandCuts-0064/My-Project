using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected class FlatModFlat : ModFlat<FlatStat>
    {
        internal FlatModFlat(FlatStatType flatStatType, float modBase) : base(flatStatType) => Stat = new(modBase);

        internal override void Start() => TargetStat.ModFlat(Stat);
    }



    private protected class MultModFlat : ModFlat<MultStat>
    {
        internal MultModFlat(FlatStatType flatStatType, float modBase) : base(flatStatType) => Stat = new(modBase);

        internal override void Start() => TargetStat.ModMult(Stat);
    }



    private protected class FlatModMult : ModMult<MultStat>
    {
        internal FlatModMult(MultStatType multStatType, float modBase) : base(multStatType) => Stat = new(modBase);

        internal override void Start() => TargetStat.ModFlat(Stat);
    }



    private protected class MultModMult : ModMult<MultStat>
    {
        internal MultModMult(MultStatType multStatType, float modBase) : base(multStatType) => Stat = new(modBase);

        internal override void Start() => TargetStat.ModMult(Stat);
    }
}
