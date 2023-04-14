using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected abstract class Mod<TOwned, TTarget> : Component
    where TOwned : Stat
    where TTarget : Stat
    {
        public TOwned Stat { get; protected set; }
        public TTarget TargetStat { get; protected set; }

        internal override Component Clone() => this;

        internal override void Stop() => Stat.RemoveFromOthers();
    }



    private protected abstract class ModFlat<TOwned> : Mod<TOwned, FlatStat>
    where TOwned : Stat
    {
        public FlatStatType FlatStatType { get; set; }

        internal ModFlat(FlatStatType flatStatType) => FlatStatType = flatStatType;

        internal override bool TryInitialize(Stats stats)
        {
            if (!stats.TryGetStat(FlatStatType, out FlatStat flatStat))
                return false;

            TargetStat = flatStat;
            return true;
        }
    }



    private protected abstract class ModMult<TOwned> : Mod<TOwned, MultStat>
    where TOwned : Stat
    {
        public MultStatType MultStatType { get; set; }

        internal ModMult(MultStatType multStatType) => MultStatType = multStatType;

        internal override bool TryInitialize(Stats stats)
        {
            if (!stats.TryGetStat(MultStatType, out MultStat multStat))
                return false;

            TargetStat = multStat;
            return true;
        }
    }
}
