using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.Netcode;

public class Stats : NetworkBehaviour, IReadOnlyStats
{
    [SerializeReference] private List<Resource> _resources; 
    public IReadOnlyList<IReadOnlyResource> Resources => _resources;

    private void FixedUpdate()
    {
        foreach (var resource in _resources)
            resource.Current += resource.Generation * Time.fixedDeltaTime;
    }

    public void TakeDamage(Element element, float damage)
    {

    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Stats), true)]
    protected class Editor : UnityEditor.Editor
    {
        private int _selectedElementIndex;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Label("\nNew " + nameof(Resource) + ":");

            _selectedElementIndex = EditorGUILayout.Popup(_selectedElementIndex, Enum.GetNames(typeof(Element)));

            if (_selectedElementIndex != 0)
            {
                ((Stats)target)._resources.Add(new Resource((Element)_selectedElementIndex));
                _selectedElementIndex = 0;
            }
        }
    }

#endif

}
