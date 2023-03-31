using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

[Serializable]
internal class Resource : IReadOnlyResource
{
#if UNITY_EDITOR
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for serialization")]
    [SerializeField, HideInInspector] private string _name;
#endif
    [SerializeField] private FlatStat _max;
    [SerializeField] private float _current;
    [field: SerializeField, HideInInspector] public Element Element { get; private set; }

    public float Max => _max.Value;

    public float Current
    {
        get => _current;
        set
        {
            if (value >= Max)
                if (Current == Max) return;
                else value = Max;

            _current = value;
            CurrentChanged?.Invoke(_current);
        }
    }

    public event Action<float> CurrentChanged;

    public Resource(Element element)
    {
        Element = element;
#if UNITY_EDITOR
        _name = element.ToString();
#endif
    }

    public void ModFlat(FlatStat stat)
    {
        _max.ModFlat(stat);
        if (Current > Max)
            Current = Max;
    }

    public void ModMult(MultStat stat)
    {
        _max.ModMult(stat);
        if (Current > Max)
            Current = Max;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Resource))]
    class Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Event.current.type == EventType.Layout) return;

            SerializedProperty name = property.FindPropertyRelative(nameof(_name));
            SerializedProperty current = property.FindPropertyRelative(nameof(_current));
            SerializedProperty max = property.FindPropertyRelative(nameof(_max));
            SerializedProperty maxValue = max.FindPropertyRelative(Stat.VALUE_FIELD_NAME);

            position.height /= GetRows(property);
            Vector2 mousePosition = Event.current.mousePosition;
            if (Event.current.isMouse
                && Event.current.button == 0
                && position.Contains(mousePosition))
                current.floatValue = Mathf.Round((mousePosition.x - position.x) / position.width * maxValue.floatValue);

            CustomGUI.Bar(
                position,
                Enum.Parse<Element>(name.stringValue).GetColor(),
                current.floatValue / maxValue.floatValue,
                Utility.ClearedStringBuilder
                    .Append(name.stringValue)
                    .Append(" (")
                    .Append(current.floatValue)
                    .Append("/")
                    .Append(maxValue.floatValue)
                    .Append(")")
                    .ToString());

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, (string)null))
            {
                position.y += position.height;
                EditorGUI.PropertyField(position, max);
                if (current.floatValue > maxValue.floatValue)
                    current.floatValue = maxValue.floatValue;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            base.GetPropertyHeight(property, label) * GetRows(property);

        private int GetRows(SerializedProperty property) => property.isExpanded ? 2 : 1;
    }
#endif
}
