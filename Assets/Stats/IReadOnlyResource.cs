using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyResource
{
    public Element Element { get; }
    public float Max { get; }
    public float Current { get; }
    public float Generation { get; }

    event Action<float> MaxChanged;
    event Action<float> CurrentChanged;
    event Action<float> GenerationChanged;
}
