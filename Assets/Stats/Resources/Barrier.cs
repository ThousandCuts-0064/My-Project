using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Resource
{
    [SerializeField] private float _effectiveness;

    public override Color Color => Color.yellow;

    public float Effectiveness
    {
        get => _effectiveness;
        set
        {
            _effectiveness = value;
            EffectivenessChanged?.Invoke(_effectiveness);
        }
    }

    public event Action<float> EffectivenessChanged;
}
