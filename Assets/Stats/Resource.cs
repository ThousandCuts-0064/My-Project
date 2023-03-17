using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[Serializable]
internal class Resource : IReadOnlyResource
{
#if UNITY_EDITOR
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for serialization")]
    [SerializeField, HideInInspector] private string _name;
#endif
    [SerializeField] private float _max;
    [SerializeField] private float _current;
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

    public event Action<float> MaxChanged;
    public event Action<float> CurrentChanged;

    public Resource(Element element)
    {
        Element = element;
#if UNITY_EDITOR
        _name = element.ToString();
        CurrentChanged += curr => SetName();
        MaxChanged += max => SetName();

        void SetName() => _name = Utility.ClearedStringBuilder
            .Append(element)
            .Append(" (")
            .Append(Current)
            .Append("/")
            .Append(Max)
            .Append(" )")
            .ToString();
#endif
    }
}
