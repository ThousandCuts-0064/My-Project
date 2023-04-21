using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupY : Group
{
    protected override float SelectDimension(Vector2 parentSize) => parentSize.y;

    protected override Vector2 ScaleSize(int scale) => 
        new(RectTransform.rect.width, ScaleUnit * scale);

    protected override Vector2 FindPosition(int scale, int acumulated) =>
        new (RectTransform.rect.width / 2, -(ScaleUnit * scale) / 2 - acumulated * ScaleUnit);
}
