using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Stat : IReadOnlyStat
#if UNITY_EDITOR
    , ISerializationCallbackReceiver
#endif
{
#if UNITY_EDITOR
    private float _oldBase;
#endif
    private readonly HashSet<Stat> _flatMods = new();
    private readonly HashSet<Stat> _multMods = new();
    private readonly HashSet<Stat> _modOfOthers = new();
    [field: SerializeField] public float Base { get; private set; }
    [field: SerializeField] public float Value { get; private set; }

    public Stat(float baseValue) => Base = baseValue;

    public void ModFlat(Stat stat)
    {
        _flatMods.Add(stat);
        stat._modOfOthers.Add(this);
    }

    public void ModMult(Stat stat)
    {
        _multMods.Add(stat);
        stat._modOfOthers.Add(this);
    }

    public void RemoveFromOthers()
    {
        foreach (var mod in _modOfOthers)
        {
            if (mod._flatMods.Remove(this)) continue;
            if (mod._multMods.Remove(this)) continue;
        }
    }

    private void Calculate()
    {
        Value = Base;
    }

#if UNITY_EDITOR
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        if (Base == _oldBase)
            return;

        Calculate();
        _oldBase = Base;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize() { }

    [CustomPropertyDrawer(typeof(Stat))]
    public class Drawer : PropertyDrawer
    {
        private static readonly GUIStyle _style = CustomGUI.CenteredLabel;
        public float RectNameDenominator { get; set; } = 2.85f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Event.current.type == EventType.Layout) return;

            int fieldCount = 2;
            int rectCount = fieldCount * 2;
            Span<Rect> rects = stackalloc Rect[rectCount];
            Rect rectName = new(position.position, new Vector2(position.width / RectNameDenominator, position.height));
            Vector2 size = new((position.width - rectName.width) / rectCount, position.height);

            for (int i = 0; i < rectCount; i++)
                rects[i] = new(rectName.x + rectName.width + size.x * i, position.y, size.x, size.y);

            EditorGUI.LabelField(rectName, Utility.SeparateWords(Utility.NameFromField(property.name)), EditorStyles.boldLabel);
            EditorGUI.indentLevel = 0;

            SerializedProperty @base = property.FindPropertyRelative(Utility.ToBackingField(nameof(Base)));
            SerializedProperty value = property.FindPropertyRelative(Utility.ToBackingField(nameof(Value)));
            EditorGUI.LabelField(rects[0], @base.displayName, _style);
            @base.floatValue = EditorGUI.DelayedFloatField(rects[1], @base.floatValue);
            GUI.enabled = false;
            EditorGUI.LabelField(rects[2], value.displayName, _style);
            value.floatValue = EditorGUI.FloatField(rects[3], value.floatValue);
            GUI.enabled = true;
        }
    }
#endif
}
