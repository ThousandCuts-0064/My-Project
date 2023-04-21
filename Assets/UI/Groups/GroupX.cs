using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupX : Group
{
    protected override float SelectDimension(Vector2 parentSize) => parentSize.x;

    protected override Vector2 ScaleSize(int scale) =>
        new(ScaleUnit * scale, RectTransform.rect.height);

    protected override Vector2 FindPosition(int scale, int acumulated) =>
        new(ScaleUnit * scale / 2 + acumulated * ScaleUnit, -RectTransform.rect.height / 2);
}
