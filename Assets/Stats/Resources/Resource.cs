using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource : ScriptableObject, IReadOnlyResource
{
    [SerializeField] private float _max;
    [SerializeField] private float _current;
    [SerializeField] private float _generation;

    public abstract Color Color { get; }

    public float Max
    {
        get => _max;
        set
        {
            _max = value;
            MaxChanged?.Invoke(_max);
        }
    }

    public float Current
    {
        get => _current;
        set
        {
            if (value >= Max)
                if (Current == Max) return;
                else value = Max;

            _current = value;
            CurrentChanged?.Invoke(_current);
        }
    }

    public float Generation 
    {
        get => _generation;
        set
        {
            _generation = value;
            GenerationChanged?.Invoke(_generation);
        }
    }

    public event Action<float> MaxChanged;
    public event Action<float> CurrentChanged;
    public event Action<float> GenerationChanged;
}
