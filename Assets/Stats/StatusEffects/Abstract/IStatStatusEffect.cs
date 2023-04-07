using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatStatusEffect<out T> where T : Stat
{
    public T Stat { get; }
}
