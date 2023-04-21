using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class GameManager : MonoBehaviour
{
#if UNITY_EDITOR
    private static readonly List<(string text, Action action)> _buttons = new();
#endif
    public static event Action EditorUpdateEvent;
    public static event Action UpdateEvent;
    public static event Action FixedUpdateEvent;

    private void Update()
    {
        if (Application.IsPlaying(gameObject))
            UpdateEvent?.Invoke();
        else
            EditorUpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        FixedUpdateEvent?.Invoke();
    }

#if UNITY_EDITOR
    public static void MakeButton(string text, Action action) => _buttons.Add((text, action));

    [CustomEditor(typeof(GameManager))]
    class Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            foreach (var (text, action) in _buttons)
                if (GUILayout.Button(text))
                    action();
        }
    }
#endif
}
