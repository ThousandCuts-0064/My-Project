using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected abstract class Mod<TOwned, TTarget> : Component
    where TOwned : Stat
    where TTarget : Stat
    {
        protected TOwned Stat { get; set; }
        protected TTarget TargetStat { get; set; }

        internal override Component Clone() => this;

        internal override void Stop() => Stat.RemoveFromOthers();
    }



    private protected abstract class ModFlat<TOwned> : Mod<TOwned, FlatStat>
    where TOwned : Stat
    {
        private readonly FlatStatType _flatStatType;

        internal ModFlat(FlatStatType flatStatType) => _flatStatType = flatStatType;

        internal override bool TryInitialize(Stats stats)
        {
            if (!stats.TryGetStat(_flatStatType, out FlatStat flatStat))
                return false;

            TargetStat = flatStat;
            return true;
        }
    }



    private protected abstract class ModMult<TOwned> : Mod<TOwned, MultStat>
    where TOwned : Stat
    {
        private readonly MultStatType _multStatType;

        internal ModMult(MultStatType multStatType) => _multStatType = multStatType;

        internal override bool TryInitialize(Stats stats)
        {
            if (!stats.TryGetStat(_multStatType, out MultStat multStat))
                return false;

            TargetStat = multStat;
            return true;
        }
    }
}
