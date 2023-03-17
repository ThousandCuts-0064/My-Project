using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyCharacterStats
{
    IReadOnlyStat MovementSpeed { get; }
    IReadOnlyStat JumpStrength { get; }
}
