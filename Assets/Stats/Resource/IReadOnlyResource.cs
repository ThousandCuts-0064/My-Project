using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyResource
{
    public Element Element { get; }
    public float Max { get; }
    public float Current { get; }

    event Action<float> CurrentChanged;
}
