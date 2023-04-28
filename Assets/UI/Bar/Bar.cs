using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Image _background;
    private Image _fill;
    [SerializeField, Range(0, 1)] private float _value;
    [SerializeField] private bool _isVertical = true;

    public Color BackgroundColor { get => _background.color; set => _background.color = value; }
    public Color FillColor { get => _fill.color; set => _fill.color = value; }
    public float Value
    {
        get => _value;
        set
        {
            if (_value == value)
                return;

            _value = value;
            UpdateFill();
        }
    }
    public bool IsVertical
    {
        get => _isVertical;
        set
        {
            if (_isVertical == value)
                return;

            _isVertical = value;
            UpdateFill();
        }
    }
    private void Awake()
    {
        _rectTransform = (RectTransform)transform;
        _background = transform.GetChild(0).GetComponent<Image>();
        _fill = transform.GetChild(1).GetComponent<Image>();
    }

    private void UpdateFill() => _fill.rectTransform.offsetMax = IsVertical
            ? new Vector2(0, _rectTransform.rect.height * (Value - 1))
            : new Vector2(_rectTransform.rect.width * (Value - 1), 0);

#if UNITY_EDITOR
    [CustomEditor(typeof(Bar), true)]
    private class Editor : UnityEditor.Editor
    {
        private Bar _bar;
        private float _oldValue;
        private bool _oldIsVertical;

        private void Awake()
        {
            _bar = (Bar)target;
            if (_bar._fill == null)
                _bar.Awake();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (_oldValue == _bar.Value && _oldIsVertical == _bar.IsVertical)
                return;

            _bar.UpdateFill();
            _oldValue = _bar.Value;
            _oldIsVertical = _bar.IsVertical;
        }
    }
#endif
}
