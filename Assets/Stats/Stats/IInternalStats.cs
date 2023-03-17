using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal interface IInternalStats
{
    IReadOnlyList<Resource> Externals { get; }
    IReadOnlyList<Resource> Internals { get; }
    IReadOnlyList<Resource> Embedded { get; }
}
