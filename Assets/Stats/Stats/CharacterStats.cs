using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterStats : Stats, IReadOnlyCharacterStats
{
    [field: SerializeField] private Stat _movementSpeed;
    [field: SerializeField] private Stat _jumpStrength;
    public IReadOnlyStat MovementSpeed => _movementSpeed;
    public IReadOnlyStat JumpStrength => _jumpStrength;
}
