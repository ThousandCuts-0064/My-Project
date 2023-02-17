using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyResource
{
    public Color Color { get; }
    public float Max { get; }
    public float Current { get; }
    public float Generation { get; }
}
