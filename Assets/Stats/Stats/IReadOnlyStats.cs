using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReadOnlyStats
{
    IReadOnlyList<IReadOnlyResource> Externals { get; }
    IReadOnlyList<IReadOnlyResource> Internals { get; }
    IReadOnlyList<IReadOnlyResource> Embedded { get; }
}
