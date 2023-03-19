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

    internal IReadOnlyList<Resource> ExternalsInternal => _externals;
    internal IReadOnlyList<Resource> InternalsInternal => _externals;
    internal IReadOnlyList<Resource> EmbeddedInternal => _externals;

    public IReadOnlyList<IReadOnlyResource> Externals => _externals;
    public IReadOnlyList<IReadOnlyResource> Internals => _externals;
    public IReadOnlyList<IReadOnlyResource> Embedded => _externals;

    private void Awake()
    {
        foreach (var effect in _statusEffects)
            effect.Start(this);

        foreach (var effect in _temporaryStatusEffects)
            effect.Start(this);
    }

    public virtual void AddStatusEffect(StatusEffect statusEffect)
    {
        (statusEffect is TemporaryStatusEffect
            ? _temporaryStatusEffects
            : _statusEffects)
            .Add(statusEffect);

        statusEffect.Start(this);
    }

    public void TakeDamage(Element element, float damage)
    {

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Stats), true)]
    private class Editor : UnityEditor.Editor
    {
        private static readonly FieldInfo[] _resourceListsField;
        private static readonly string[] _resourceListsName;
        private static readonly Type[] _statusEffectsType;
        private static readonly ConstructorInfo[] _statusEffectsConstructorInfo;
        private static readonly string[] _statusEffectsName;
        private int _resourceListIndex;
        private int _resourceElementIndex;
        private int _statusEffectIndex;
        private int _statusEffectElementIndex;
        private float _statusEffectDuration;

        static Editor()
        {
            _resourceListsField = typeof(Stats)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(f => typeof(ICollection<Resource>).IsAssignableFrom(f.FieldType))
                .ToArray();

            _resourceListsName = _resourceListsField
                .Select(f => char.ToUpper(f.Name[1]) + f.Name[2..])
                .ToArray();

            _statusEffectsType = typeof(StatusEffect).Assembly
                .GetTypes()
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(StatusEffect)))
                .ToArray();

            _statusEffectsConstructorInfo = _statusEffectsType
                .Select(t => t.GetConstructors().Single())
                .ToArray();

            _statusEffectsName = _statusEffectsType
                .Select(t => t.Name)
                .ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Stats stats = (Stats)target;

            GUILayout.Label("\n");
            GUILayout.BeginHorizontal();
            GUILayout.Label("New " + nameof(Resource) + ":");
            _resourceListIndex = EditorGUILayout.Popup(_resourceListIndex, _resourceListsName);
            _resourceElementIndex = EditorGUILayout.Popup(_resourceElementIndex, Enum.GetNames(typeof(Element)));
            if (_resourceElementIndex != 0)
            {
                ((ICollection<Resource>)_resourceListsField[_resourceListIndex].GetValue(stats)).Add(new Resource((Element)_resourceElementIndex));
                _resourceElementIndex = 0;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("New " + nameof(StatusEffect) + ":", GUILayout.ExpandWidth(false));
            _statusEffectIndex = EditorGUILayout.Popup(_statusEffectIndex, _statusEffectsName);
            Type selectedType = _statusEffectsType[_statusEffectIndex];
            bool isElemental = selectedType.GetInterfaces().Contains(typeof(IElementalStatusEffect));
            bool isTemporary = selectedType.IsSubclassOf(typeof(TemporaryStatusEffect));
            _statusEffectElementIndex = isElemental ? EditorGUILayout.Popup(_statusEffectElementIndex, Enum.GetNames(typeof(Element))) : 0;
            _statusEffectDuration = isTemporary ? EditorGUILayout.FloatField(_statusEffectDuration) : 0;
            if (GUILayout.Button("Add"))
                (isTemporary
                    ? stats._temporaryStatusEffects
                    : stats._statusEffects)
                    .Add((StatusEffect)_statusEffectsConstructorInfo[_statusEffectIndex].Invoke(null));
            GUILayout.EndHorizontal();
        }
    }
#endif
}