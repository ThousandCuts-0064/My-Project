using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    None      = 0,

    Earth     = 1 << 0,
    Water     = 1 << 1,
    Air       = 1 << 2,
    Fire      = 1 << 3,

    Light     = 1 << 4,
    Life      = 1 << 5,
    Spirit    = 1 << 6,
    Decay     = 1 << 7,
              
    Stone     = 1 << 8,
    Metal     = 1 << 9,
    Sand      = 1 << 10,
    Oil       = 1 << 11,
    Crystal   = 1 << 12,
              
    Ice       = 1 << 13,
    Vapour    = 1 << 14,
              
    Smoke     = 1 << 15,
    Lightning = 1 << 16,

    Lava      = 1 << 17,
    Glass     = 1 << 18,

    Blood     = 1 << 19,
    Plant     = 1 << 20,

    Mana      = 1 << 21,
}
