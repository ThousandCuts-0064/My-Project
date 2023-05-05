using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFinishers
{
    public StatusEffect Timer(float time);
    public StatusEffect Permanent();
}
