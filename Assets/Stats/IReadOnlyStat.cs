using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyStat
{
    float Base { get; }
    float Value { get; }
}
