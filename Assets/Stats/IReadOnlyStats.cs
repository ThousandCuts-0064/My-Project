using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyStats
{
    IReadOnlyList<IReadOnlyResource> Resources { get; }
}
