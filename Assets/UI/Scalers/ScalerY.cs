using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerY : Scaler
{
    protected override float SelectDimension(Vector2 parentSize) => parentSize.y;

    protected override void Scale(RectTransform rectTransform, int acumulated, int scale)
    {
        rectTransform.anchoredPosition = new Vector2(0, rectTransform.sizeDelta.y / (IsChildCountOdd ? 1 : 2) - acumulated * ScaleSize);
        rectTransform.sizeDelta = new Vector2(((RectTransform)rectTransform.parent).rect.width, ScaleSize * scale);
    }
}
