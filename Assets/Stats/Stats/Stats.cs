using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Unity.Netcode;
using System.Reflection;

[DisallowMultipleComponent]
public class Stats : NetworkBehaviour, IReadOnlyStats
{
    [SerializeReference] private List<Resource> _externals;
    [SerializeReference] private List<Resource> _internals;
    [SerializeReference] private List<Resource> _embedded;
    [SerializeReference] private List<StatusEffect> _statusEffects;
    [SerializeReference] private List<StatusEffect> _temporaryStatusEffects;
    public IReadOnlyList<IReadOnlyResource> Externals => _externals;
    public IReadOnlyList<IReadOnlyResource> Internals => _externals;
    public IReadOnlyList<IReadOnlyResource> Embedded => _externals;

    private void Awake()
    {
        foreach (var effect in _statusEffects)
            effect.Start();

        foreach (var effect in _temporaryStatusEffects)
            effect.Start();
    }

    public virtual void AddStatusEffect(StatusEffect statusEffect)
    {
        (statusEffect.IsTemporary 
            ? _temporaryStatusEffects 
            : _statusEffects)
                .Add(statusEffect);

        statusEffect.Start();
    }
        
    public void TakeDamage(Element element, float damage)
    {

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Stats), true)]
    private class Editor : UnityEditor.Editor
    {
        private static readonly FieldInfo[] _resourceFields;
        private static readonly string[] _resourceFieldsName;
        private int _selectedResourcePlaceIndex;
        private int _selectedElementIndex;

        static Editor()
        {
            _resourceFields = typeof(Stats)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(f => typeof(ICollection<Resource>).IsAssignableFrom(f.FieldType))
                .ToArray();

            _resourceFieldsName = _resourceFields
                .Select(f => char.ToUpper(f.Name[1]) + f.Name[2..])
                .ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Label("\nNew " + nameof(Resource) + ":");

            _selectedResourcePlaceIndex = EditorGUILayout.Popup(_selectedResourcePlaceIndex, _resourceFieldsName);

            _selectedElementIndex = EditorGUILayout.Popup(_selectedElementIndex, Enum.GetNames(typeof(Element)));

            if (_selectedElementIndex != 0)
            {
                ((ICollection<Resource>)_resourceFields[_selectedResourcePlaceIndex].GetValue(target)).Add(new Resource((Element)_selectedElementIndex));
                _selectedElementIndex = 0;
            }
        }
    }
#endif
}
