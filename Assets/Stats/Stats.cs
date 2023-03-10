using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(fileName = nameof(Stats), menuName = nameof(ScriptableObject) + "/" + nameof(Stats) + "/" + nameof(Stats))]
public class Stats : ScriptableObject, IReadOnlyStats
{
    [SerializeReference] private List<Resource> _resources;
    public IReadOnlyList<IReadOnlyResource> Resources => _resources;

    public void FixedUpdate()
    {
        foreach (var resource in _resources)
            resource.Current += resource.Generation * Time.fixedDeltaTime;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Stats), true)]
    protected class Editor : UnityEditor.Editor
    {
        private static readonly Type[] _resourceTypes;
        private int _selectedResourceTypeIndex;

        static Editor() => _resourceTypes = typeof(Resource).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Resource))).ToArray();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _selectedResourceTypeIndex = EditorGUILayout.Popup(_selectedResourceTypeIndex, _resourceTypes.Select(t => t.Name).ToArray());

            if (GUILayout.Button("Add"))
                ((Stats)target)._resources.Add((Resource)Activator.CreateInstance(_resourceTypes[_selectedResourceTypeIndex]));
        }
    }

#endif

}
