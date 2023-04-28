using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterStats : Stats
{
    [SerializeField] private FlatStat _movementSpeed;
    [SerializeField] private FlatStat _jumpStrength;

    public IReadOnlyStat MovementSpeed => MovementSpeed;
    public IReadOnlyStat JumpStrength => JumpStrength;

    internal override bool TryGetStat(FlatStatType flatStatType, out FlatStat stat)
    {
        stat = flatStatType switch
        {
            FlatStatType.MovementSpeed => _movementSpeed,
            FlatStatType.JumpStrength => _jumpStrength,
            _ => null
        };
        return stat != null;
    }
}
