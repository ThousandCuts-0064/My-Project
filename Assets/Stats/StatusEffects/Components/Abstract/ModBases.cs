using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    [Serializable]
    private protected abstract class _Mod<TOwned, TTarget> : _Component
    where TOwned : Stat
    where TTarget : Stat
    {
        protected TOwned Stat { get; set; }
        protected TTarget TargetStat { get; set; }

        internal override void Stop() => Stat.RemoveFromOthers();
    }



    [Serializable]
    private protected abstract class _ModFlat<TOwned> : _Mod<TOwned, FlatStat>
    where TOwned : Stat
    {
        protected FlatStatType FlatStatType { get; }

        internal _ModFlat(FlatStatType flatStatType) => FlatStatType = flatStatType;

        internal override bool TryInitialize(Stats stats)
        {
            if (!stats.TryGetStat(FlatStatType, out FlatStat flatStat))
                return false;

            TargetStat = flatStat;
            return true;
        }
    }



    [Serializable]
    private protected abstract class _ModMult<TOwned> : _Mod<TOwned, MultStat>
    where TOwned : Stat
    {
        protected MultStatType MultStatType { get; }

        internal _ModMult(MultStatType multStatType) => MultStatType = multStatType;

        internal override bool TryInitialize(Stats stats)
        {
            if (!stats.TryGetStat(MultStatType, out MultStat multStat))
                return false;

            TargetStat = multStat;
            return true;
        }
    }
}
