using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterStats : Stats, IReadOnlyCharacterStats
{
    [SerializeField] private Stat _movementSpeed;
    [SerializeField] private Stat _jumpStrength;

    internal Stat MovementSpeedInternal => _movementSpeed;
    internal Stat JumpStrengthInternal => _jumpStrength;

    public IReadOnlyStat MovementSpeed => MovementSpeed;
    public IReadOnlyStat JumpStrength => JumpStrength;
}
