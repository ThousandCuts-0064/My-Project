using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;

[ExecuteAlways]
public abstract class Scaler : UIBehaviour
#if UNITY_EDITOR
    , ISerializationCallbackReceiver
#endif
{
#if UNITY_EDITOR
    private RectTransform[] _rectTransforms;
    private int[] _scales;
#endif
    private Dictionary<RectTransform, int> _rectToScale;
    private RectTransform _rectTransform;
    private Vector2 _oldSize;
    protected float ScaleSize { get; private set; }
    protected bool IsChildCountOdd => _rectToScale.Count % 2 != 0;
    [field: SerializeField] protected int ScaleSum { get; private set; }

    protected override void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _oldSize = _rectTransform.sizeDelta;
        _rectToScale = new Dictionary<RectTransform, int>(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform rectTransform = (RectTransform)transform.GetChild(i);
            _rectToScale.Add(rectTransform, 1);
            rectTransform.anchorMin = new(0.5f, 0.5f);
            rectTransform.anchorMax = new(0.5f, 0.5f);
            rectTransform.pivot = new(0.5f, 0.5f);
        }
        ScaleSum = transform.childCount;
        UpdatePartSize();

        transform.root.GetComponent<CanvasTracker>().RectTransformDimensionsChange += UpdateScale;
    }

    protected override void OnDestroy() => 
        transform.root.GetComponent<CanvasTracker>().RectTransformDimensionsChange -= UpdateScale;

    protected abstract float SelectDimension(Vector2 parentSize);
    protected abstract void Scale(RectTransform rectTransform, int acumulated, int scale);

    private void Update()
    {
        if (_rectTransform.sizeDelta != _oldSize)
        {
            UpdateScale();
            _oldSize = _rectTransform.sizeDelta;
            return;
        }
    }

    private void OnTransformChildrenChanged()
    {
        ScaleSum = 0;
        Dictionary<RectTransform, int> newRectToScale = new(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform rect = (RectTransform)transform.GetChild(i);
            newRectToScale.Add(rect, _rectToScale.TryGetValue(rect, out int scale) ? scale : ++scale);
            ScaleSum += scale;
        }
        UpdatePartSize();
        _rectToScale = newRectToScale;
        UpdateScale();
    }

    private void UpdatePartSize() => ScaleSize = SelectDimension(_rectTransform.rect.size) / ScaleSum;

    private void UpdateScale()
    {
        int acumulate = 0;
        foreach (var pair in _rectToScale)
        {
            Scale(pair.Key, acumulate, pair.Value);
            acumulate += pair.Value;
        }
    }

#if UNITY_EDITOR
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
        for (int i = 0; i < _scales.Length; i++)
            _rectToScale.Add(_rectTransforms[i], _scales[i]);
    }

    [CustomEditor(typeof(Scaler), true)]
    private class Editor : UnityEditor.Editor
    {
        private bool _isFolded = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Scaler uiScaler = (Scaler)target;

            bool shouldUpdateScale = false;
            EditorGUILayout.Space();
            if (_isFolded = EditorGUILayout.Foldout(_isFolded, "Scales"))
            {
                uiScaler.ScaleSum = 0;
                foreach (var rectScale in uiScaler._rectToScale.ToList())
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(rectScale.Key.name);
                    int oldScale = rectScale.Value;
                    int scale = EditorGUILayout.IntField(rectScale.Value);
                    if (scale != oldScale)
                        shouldUpdateScale = true;
                    uiScaler._rectToScale[rectScale.Key] = scale;
                    uiScaler.ScaleSum += scale;
                    EditorGUILayout.EndHorizontal();
                }
                uiScaler.UpdatePartSize();
                if (shouldUpdateScale)
                    uiScaler.UpdateScale();
            }
        }
    }
#endif
}
