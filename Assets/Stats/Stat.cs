using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Stat : IReadOnlyStat
{
    private readonly HashSet<Stat> _flatMods;
    private readonly HashSet<Stat> _multMods;
    private readonly HashSet<Stat> _modOfOthers;
    [field: SerializeField] public float BaseValue { get; private set; }
    public float Value => BaseValue;

    public Stat(float baseValue) => BaseValue = baseValue;

    public void AddModFlat(Stat stat)
    {
        _flatMods.Add(stat);
        stat._modOfOthers.Add(this);
    }

    public void AddModMult(Stat stat)
    {
        _multMods.Add(stat);
        stat._modOfOthers.Add(this);
    }
}
