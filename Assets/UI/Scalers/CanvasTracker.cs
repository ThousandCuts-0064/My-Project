using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteAlways]
public class CanvasTracker : UIBehaviour
{
    public event Action RectTransformDimensionsChange;

    protected override void OnRectTransformDimensionsChange() => RectTransformDimensionsChange?.Invoke();
}
