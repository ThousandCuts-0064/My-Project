using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterStats : Stats, IReadOnlyCharacterStats
{
    [SerializeField] private FlatStat _movementSpeed;
    [SerializeField] private FlatStat _jumpStrength;

    internal FlatStat MovementSpeedInternal => _movementSpeed;
    internal FlatStat JumpStrengthInternal => _jumpStrength;

    public IReadOnlyStat MovementSpeed => MovementSpeed;
    public IReadOnlyStat JumpStrength => JumpStrength;

    internal override bool TryGetFlatStat(string name, out FlatStat stat)
    {
        stat = name switch
        {
            nameof(MovementSpeed) => MovementSpeedInternal,
            nameof(JumpStrength) => JumpStrengthInternal,
            _ => null
        };
        return stat == null;
    }
}
