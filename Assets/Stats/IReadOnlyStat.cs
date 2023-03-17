using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyStat
{
    float BaseValue { get; }
    float Value { get; }
}
