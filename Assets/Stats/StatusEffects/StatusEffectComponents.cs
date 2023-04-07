using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected abstract class Component
    {
        internal abstract bool TryInitialize(Stats stats);
        internal abstract void Start();
        internal abstract void Stop();
    }

    private protected abstract class Mod<TOwned, TTarget> : Component
    where TOwned : Stat
    where TTarget : Stat
    {
        public TTarget TargetStat { get; protected set; }
        public TOwned Stat { get; }
        public string StatName { get; set; }

        internal override void Stop() => Stat.RemoveFromOthers();
    }

    private protected class FlatModFlat : Mod<FlatStat, FlatStat>
    {
        internal override bool TryInitialize(Stats stats)
        {
            if (!stats.TryGetFlatStat(StatName, out FlatStat stat))
                return false;

            TargetStat = stat;
            return true;
        }

        internal override void Start() => TargetStat.ModFlat(Stat);
    }

    private protected class MultModFlat : Mod<MultStat, FlatStat>
    {
        internal override bool TryInitialize(Stats stats)
        {
            if (!stats.TryGetFlatStat(StatName, out FlatStat stat))
                return false;

            TargetStat = stat;
            return true;
        }

        internal override void Start() => TargetStat.ModMult(Stat);
    }
}
