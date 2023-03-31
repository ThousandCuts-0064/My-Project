using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static event Action UpdateEvent;
    public static event Action FixedUpdateEvent;

    [field: SerializeField] public GameObject[] ForceAwakes { get; private set; }

    private void Awake()
    {
        for (int i = 0; i < ForceAwakes.Length; i++)
            ForceAwakes[i].SetActive(true);
    }

    private void Update()
    {
        UpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        FixedUpdateEvent?.Invoke();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameManager))]
    class Editor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Update Element Colors"))
                Debug.Log(1);

        }
    }
#endif
}
