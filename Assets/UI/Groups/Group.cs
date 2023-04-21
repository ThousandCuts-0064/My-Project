using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;

[ExecuteAlways]
public abstract class Group : UIBehaviour, ISerializationCallbackReceiver
{
    private Dictionary<RectTransform, int> _rectToScale;
    [SerializeField, HideInInspector] private RectTransform[] _rectTransforms;
    [SerializeField, HideInInspector] private int[] _scales;
    protected RectTransform RectTransform { get; private set; }
    protected float ScaleUnit { get; private set; }
    protected int ScaleSum { get; private set; }

    protected override void Awake()
    {
        RectTransform = (RectTransform)transform;
        if (_scales is not null)
            return;

        _rectToScale = new Dictionary<RectTransform, int>(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform rectTransform = (RectTransform)transform.GetChild(i);
            _rectToScale.Add(rectTransform, 1);
            SetAnchores(rectTransform);
        }
        ScaleSum = transform.childCount;
        UpdateScale();
    }

    protected abstract float SelectDimension(Vector2 parentSize);
    protected abstract Vector2 ScaleSize(int scale);
    protected abstract Vector2 FindPosition(int scale, int acumulated);

    protected override void OnRectTransformDimensionsChange()
    {
        if (RectTransform is null)
            return;

        UpdateScale();
    }

    private void OnTransformChildrenChanged()
    {
        GameManager.EditorUpdateEvent += Method;
        void Method()
        {
            ScaleSum = 0;
            Dictionary<RectTransform, int> newRectToScale = new(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform rect = (RectTransform)transform.GetChild(i);
                SetAnchores(rect);
                newRectToScale.Add(rect, _rectToScale.TryGetValue(rect, out int scale) ? scale : ++scale);
                ScaleSum += scale;
            }
            _rectToScale = newRectToScale;
            UpdateScale();
            GameManager.EditorUpdateEvent -= Method;
        }
    }

    private void SetAnchores(RectTransform rectTransform)
    {
        rectTransform.anchorMin = new(0, 1);
        rectTransform.anchorMax = new(0, 1);
        rectTransform.pivot = new(0.5f, 0.5f);
    }

    private void UpdateScale()
    {
        ScaleUnit = SelectDimension(RectTransform.rect.size) / ScaleSum;
        int acumulate = 0;
        foreach (var pair in _rectToScale)
        {
            Scale(pair.Key, pair.Value, acumulate);
            acumulate += pair.Value;

            void Scale(RectTransform rectTransform, int scale, int acumulated)
            {
                rectTransform.sizeDelta = ScaleSize(scale);
                rectTransform.anchoredPosition = FindPosition(scale, acumulated);
            }
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        _rectTransforms = new RectTransform[_rectToScale.Count];
        _scales = new int[_rectToScale.Count];
        int i = 0;
        foreach (var pair in _rectToScale)
        {
            _rectTransforms[i] = pair.Key;
            _scales[i] = pair.Value;
            i++;
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        _rectToScale = new(_scales.Length);
        ScaleSum = 0;
        for (int i = 0; i < _scales.Length; i++)
        {
            _rectToScale.Add(_rectTransforms[i], _scales[i]);
            ScaleSum += _scales[i];
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Group), true)]
    private class Editor : UnityEditor.Editor
    {
        private bool _isFolded = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Group group = (Group)target;

            GUI.enabled = false;
            EditorGUILayout.FloatField(nameof(group.ScaleUnit), group.ScaleUnit);
            EditorGUILayout.IntField(nameof(group.ScaleSum), group.ScaleSum);
            GUI.enabled = true;

            bool shouldUpdateScale = false;
            EditorGUILayout.Space();
            if (_isFolded = EditorGUILayout.Foldout(_isFolded, "Scales"))
            {
                group.ScaleSum = 0;
                foreach (var rectScale in group._rectToScale.ToList())
                {
                    if (!rectScale.Key)
                        continue;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(rectScale.Key.name);
                    int oldScale = rectScale.Value;
                    int scale = EditorGUILayout.IntField(rectScale.Value);
                    if (scale != oldScale)
                        shouldUpdateScale = true;
                    group._rectToScale[rectScale.Key] = scale;
                    group.ScaleSum += scale;
                    EditorGUILayout.EndHorizontal();
                }
                if (shouldUpdateScale)
                    group.UpdateScale();
            }
        }
    }
#endif
}
