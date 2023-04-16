using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalerX : Scaler
{
    protected override float SelectDimension(Vector2 parentSize) => parentSize.x;

    protected override void Scale(RectTransform rectTransform, int acumulated, int scale)
    {
        rectTransform.anchoredPosition = new Vector2(-rectTransform.sizeDelta.x / (IsChildCountOdd ? 1 : 2) + acumulated * ScaleSize, 0);
        rectTransform.sizeDelta = new Vector2(ScaleSize * scale, ((RectTransform)rectTransform.parent).rect.height);
    }
}
