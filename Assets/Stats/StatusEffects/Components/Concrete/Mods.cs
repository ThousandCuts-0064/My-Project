using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    [Serializable]
    private protected class _FlatModFlat : _ModFlat<FlatStat>
    {
        internal _FlatModFlat(FlatStatType flatStatType, float modBase) : base(flatStatType) => Stat = new(modBase);

        internal override _Component Clone() => new _FlatModFlat(FlatStatType, Stat.Value);
        internal override void Start() => TargetStat.ModFlat(Stat);
    }



    [Serializable]
    private protected class _MultModFlat : _ModFlat<MultStat>
    {
        internal _MultModFlat(FlatStatType flatStatType, float modBase) : base(flatStatType) => Stat = new(modBase);

        internal override _Component Clone() => new _MultModFlat(FlatStatType, Stat.Value);
        internal override void Start() => TargetStat.ModMult(Stat);
    }



    [Serializable]
    private protected class _FlatModMult : _ModMult<MultStat>
    {
        internal _FlatModMult(MultStatType multStatType, float modBase) : base(multStatType) => Stat = new(modBase);

        internal override _Component Clone() => new _FlatModMult(MultStatType, Stat.Value);
        internal override void Start() => TargetStat.ModFlat(Stat);
    }



    [Serializable]
    private protected class _MultModMult : _ModMult<MultStat>
    {
        internal _MultModMult(MultStatType multStatType, float modBase) : base(multStatType) => Stat = new(modBase);

        internal override _Component Clone() => new _MultModMult(MultStatType, Stat.Value);
        internal override void Start() => TargetStat.ModMult(Stat);
    }
}
