using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[Serializable]
public class Resource : IReadOnlyResource
{
#if UNITY_EDITOR
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for serialization")]
    [SerializeField, HideInInspector] private string _name;
#endif
    [SerializeField] private float _max;
    [SerializeField] private float _current;
    [SerializeField] private float _generation;
    [field: SerializeField, HideInInspector] public Element Element { get; private set; } 

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

    public Resource(Element element)
    {
        Element = element;
#if UNITY_EDITOR
        _name = element.ToString();
#endif
    }
}
