using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

[Serializable]
public abstract class Stat : IReadOnlyStat
#if UNITY_EDITOR
    , ISerializationCallbackReceiver
#endif
{
#if UNITY_EDITOR
    internal const string VALUE_FIELD_NAME = nameof(_value);
    private float _oldBase;
#endif
    private HashSet<Stat> _flatMods;
    private HashSet<Stat> _multMods;
    private List<Stat> _modTo;
    private bool _modsChanged;
    [SerializeField] private float _value;
    [field: SerializeField] public float Base { get; private set; }
    public float Value
    {
        get
        {
            if (_modsChanged)
            {
                Calculate();
                _modsChanged = false;
            }
            return _value;
        }
    }
    public abstract float Neutral { get; }

    internal Stat(float baseValue) => Base = baseValue;
    private protected Stat() { }

    public void RemoveFromOthers()
    {
        foreach (var mod in _modTo)
            if (!_flatMods.Remove(mod))
                _multMods.Remove(mod);
    }

    private protected void ModFlat(Stat stat)
    {
        RegisterMod(stat);
        _flatMods.Add(stat);
    }

    private protected void ModMult(Stat stat)
    {
        RegisterMod(stat);
        _multMods.Add(stat);
    }

    private void RegisterMod(Stat stat)
    {
        _modsChanged = true;
        if (_modTo is null)
        {
            _flatMods = new();
            _multMods = new();
            _modTo = new();
        }
        stat._modTo.Add(this);
    }

    private void Calculate()
    {
        _value = Base;
        foreach (var mod in _flatMods)
            _value += mod.Value;
        foreach (var mod in _multMods)
            _value *= mod.Value;
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

    [CustomPropertyDrawer(typeof(Stat), true)]
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
            SerializedProperty value = property.FindPropertyRelative(nameof(_value));
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
