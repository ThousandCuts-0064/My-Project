using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float _baseValue;
    [SerializeField] private bool _isPercentage;
    private readonly HashSet<Stat> _flatMods;
    private readonly HashSet<Stat> _multMods;
    private readonly HashSet<Stat> _modOfOthers;
    public float Value => _baseValue;

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

    public static implicit operator float(Stat stat) => stat.Value;
}
