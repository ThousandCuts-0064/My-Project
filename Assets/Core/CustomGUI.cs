using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CustomGUI
{
    private static readonly GUIStyle _centeredLabel;
    public static GUIStyle CenteredLabel => new(_centeredLabel);

    static CustomGUI()
    {
        _centeredLabel = GUI.skin.label;
        _centeredLabel.alignment = TextAnchor.MiddleCenter;
    }

    public static void Bar(Rect rect, Color fill, float value, string text) =>
        DrawBar(rect, fill, Color.black, Color.clear, value, text);
    public static void DrawBar(Rect rect, Color fill, Color boarder, Color background, float value, string text)
    {
        rect.y -= 1;
        float borderWidth = 1;
        float fillWidth = rect.width * value;
        EditorGUI.DrawRect(new Rect(rect.x , rect.y, fillWidth, rect.height), fill);
        EditorGUI.DrawRect(new Rect(rect.x + fillWidth, rect.y, rect.width - fillWidth, rect.height), background);
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, borderWidth), boarder); // Top
        EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height, rect.width, borderWidth), boarder); // Bottom
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, borderWidth, rect.height), boarder); // Left
        EditorGUI.DrawRect(new Rect(rect.x + rect.width, rect.y, borderWidth, rect.height), boarder); // Right
        Color oldColor = GUI.color;
        GUI.color = Color.black;
        EditorGUI.LabelField(rect, text, _centeredLabel);
        GUI.color = oldColor;
    }
}
