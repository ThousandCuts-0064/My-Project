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
    [SerializeReference] private List<Resource> _embedded;
    [SerializeReference] private List<Resource> _internals;
    [SerializeReference] private List<Resource> _externals;
    [SerializeReference] private List<StatusEffect> _statusEffects;
    //[SerializeReference] private List<StatusEffect> _temporaryStatusEffects;

    internal IReadOnlyList<Resource> EmbeddedInternal => _embedded;
    internal IReadOnlyList<Resource> InternalsInternal => _internals;
    internal IReadOnlyList<Resource> ExternalsInternal => _externals;

    public IReadOnlyList<IReadOnlyResource> Embedded => _embedded;
    public IReadOnlyList<IReadOnlyResource> Internals => _internals;
    public IReadOnlyList<IReadOnlyResource> Externals => _externals;

    private void Awake()
    {
        foreach (var effect in _statusEffects)
            effect.TryStart(this);
        //foreach (var effect in _temporaryStatusEffects)
        //    effect.TryStart(this);
    }

    private protected virtual List<Resource> Get(ResourceLayer resourceLayer) => resourceLayer switch
    {
        ResourceLayer.Embedded => _embedded,
        ResourceLayer.Internal => _internals,
        ResourceLayer.External => _externals,

        _ => throw EnumException.NoneOrNotDefined(nameof(resourceLayer), resourceLayer)
    };

    internal virtual IReadOnlyList<Resource> GetInternal(ResourceLayer resourceLayer) => Get(resourceLayer);

    internal virtual bool TryGetStat(FlatStatType flatStatType, out FlatStat stat)
    {
        stat = null;
        return true;
    }

    internal virtual bool TryGetStat(MultStatType multStatType, out MultStat stat)
    {
        stat = null;
        return true;
    }

    public virtual bool TryApply(StatusEffect statusEffect)
    {
        if (!statusEffect.TryStart(this))
            return false;

        //(statusEffect is TemporaryEffect
        //    ? _temporaryStatusEffects
        //    : _statusEffects)
        _statusEffects.Add(statusEffect);
        return true;
    }

    public void Take(Element element, float amount)
    {

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Stats), true)]
    private class Editor : UnityEditor.Editor
    {
        private readonly StatusEffectBuilder _statusEffectBuilder = new();
        private Stats _stats;
        private ResourceLayer _resourceLayer;
        private Element _resourceElement;

        private void Awake()
        {
            _stats = (Stats)target;
            _statusEffectBuilder.Finished += statusEffect => _stats.TryApply(statusEffect);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Label("\n");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("New " + nameof(Resource) + ":", EditorStyles.boldLabel);
            _resourceLayer = (ResourceLayer)EditorGUILayout.EnumPopup(_resourceLayer);
            _resourceElement = (Element)EditorGUILayout.EnumPopup(_resourceElement);
            EditorGUILayout.EndHorizontal();

            if (_resourceElement != Element.None)
            {
                _stats.Get(_resourceLayer).Add(new Resource(_resourceElement));
                _resourceElement = Element.None;
            }

            GUILayout.Label("\n");
            _statusEffectBuilder.OnInspectorGUI();
        }

        private class StatusEffectBuilder
        {
            private static readonly MethodInfo[] _methods;
            private static readonly string[] _methodsSelfAndParamsName;

            private ParameterInfo[] _methodParamsInfo;
            private object[] _methodParams;
            private MethodInfo _methodInfo;
            private int _methodIndex = -1;
            private int _methodIndexOld = -1;

            private StatusEffect.Builder _statusEffectBuilder;
            private StatusEffectFinisher _statusEffectFinisher;

            public event Action<StatusEffect> Finished;

            static StatusEffectBuilder()
            {
                _methods = typeof(StatusEffectPresets)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .ToArray();

                _methodsSelfAndParamsName = _methods
                    .Select(m => $"{m.Name}({string.Join(", ", m.GetParameters().Select(p => p.Name))})")
                    .ToArray();
            }

            public void OnInspectorGUI()
            {
                EditorStyles.label.fontStyle = FontStyle.Bold;
                _methodIndex = EditorGUILayout.Popup("New " + nameof(StatusEffect) + ":", _methodIndex, _methodsSelfAndParamsName);
                EditorStyles.label.fontStyle = FontStyle.Normal;

                if (_methodIndexOld != _methodIndex)
                {
                    _methodInfo = _methods[_methodIndex];
                    _methodParamsInfo = _methodInfo.GetParameters();
                    _methodParams = new object[_methodParamsInfo.Length];
                    _methodIndexOld = _methodIndex;

                    _statusEffectBuilder = (StatusEffect.Builder)_methodInfo.Invoke(null, _methodParams);
                    _statusEffectFinisher = new StatusEffectFinisher(_statusEffectBuilder);
                    _statusEffectFinisher.Finished += OnFinish;
                    _statusEffectFinisher.Cancel += Reset;
                }

                if (_methodIndex != -1)
                {
                    for (int i = 0; i < _methodParamsInfo.Length; i++)
                    {
                        ParameterInfo param = _methodParamsInfo[i];
                        if (param.IsOut)
                            continue;

                        EditorGUILayout.BeginHorizontal();
                        _methodParams[i] = param.ParameterType.IsEnum
                            ? EditorGUILayout.EnumPopup(param.Name, (Enum)(_methodParams[i] ?? Enum.ToObject(param.ParameterType, 0)))
                            : (Type.GetTypeCode(param.ParameterType) switch
                            {
                                TypeCode.Int32 => EditorGUILayout.IntField(param.Name, (int)(_methodParams[i] ?? 0)),
                                TypeCode.Single => EditorGUILayout.FloatField(param.Name, (float)(_methodParams[i] ?? 0f)),

                                _ => throw new NotImplementedException()
                            });
                        EditorGUILayout.EndHorizontal();
                    }

                    //if (_statusEffectFinisher is null)
                    //{
                    //    EditorGUILayout.BeginHorizontal();
                    //    if (GUILayout.Button("Cancel"))
                    //        Reset();

                    //    if (GUILayout.Button("Finish"))
                    //    {
                    //        _statusEffectBuilder = (StatusEffect)_methodInfo.Invoke(null, _methodParams);
                    //        OnFinish();
                    //    }
                    //    EditorGUILayout.EndHorizontal();
                    //}
                }

                EditorGUILayout.Space();
                _statusEffectFinisher?.OnInspectorGUI();
            }

            private void OnFinish(StatusEffect statusEffect)
            {
                Finished?.Invoke(statusEffect);
                Reset();
            }

            private void Reset()
            {
                _methodIndex = -1;
                _methodIndexOld = -1;
                _statusEffectFinisher = null;
            }

            class StatusEffectFinisher
            {
                private static readonly MethodInfo[] _methods;
                private static readonly string[] _methodsSelfAndParamsName;

                private readonly StatusEffect.Builder _statusEffectBuilder;

                private ParameterInfo[] _methodParamsInfo;
                private object[] _methodParams;
                private MethodInfo _methodInfo;
                private int _methodIndex = -1;
                private int _methodIndexOld = -1;

                public event Action<StatusEffect> Finished;
                public event Action Cancel;

                static StatusEffectFinisher()
                {
                    _methods = typeof(IFinishers)
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .ToArray();

                    _methodsSelfAndParamsName = _methods
                        .Select(m => $"{m.Name}({string.Join(", ", m.GetParameters().Select(p => p.Name))})")
                        .ToArray();
                }

                public StatusEffectFinisher(StatusEffect.Builder statusEffectBuilder) =>
                    _statusEffectBuilder = statusEffectBuilder;

                public void OnInspectorGUI()
                {
                    EditorStyles.label.fontStyle = FontStyle.Bold;
                    _methodIndex = EditorGUILayout.Popup("New Component:", _methodIndex, _methodsSelfAndParamsName);
                    EditorStyles.label.fontStyle = FontStyle.Normal;

                    if (_methodIndexOld != _methodIndex)
                    {
                        _methodInfo = _methods[_methodIndex];
                        _methodParamsInfo = _methodInfo.GetParameters();
                        _methodParams = new object[_methodParamsInfo.Length];
                        _methodIndexOld = _methodIndex;
                    }

                    if (_methodIndex != -1)
                    {
                        for (int i = 0; i < _methodParamsInfo.Length; i++)
                        {
                            ParameterInfo param = _methodParamsInfo[i];
                            if (param.IsOut)
                                continue;

                            EditorGUILayout.BeginHorizontal();
                            _methodParams[i] = param.ParameterType.IsEnum
                                ? EditorGUILayout.EnumPopup(param.Name, (Enum)(_methodParams[i] ?? Enum.ToObject(param.ParameterType, 0)))
                                : (Type.GetTypeCode(param.ParameterType) switch
                                {
                                    TypeCode.Int32 => EditorGUILayout.IntField(param.Name, (int)(_methodParams[i] ?? 0)),
                                    TypeCode.Single => EditorGUILayout.FloatField(param.Name, (float)(_methodParams[i] ?? 0f)),

                                    _ => throw new NotImplementedException()
                                });
                            EditorGUILayout.EndHorizontal();
                        }
                    }

                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("Cancel"))
                        Cancel?.Invoke();

                    if (GUILayout.Button("Finish"))
                        OnFinish((StatusEffect)_methodInfo.Invoke(_statusEffectBuilder, _methodParams));

                    EditorGUILayout.EndHorizontal();

                    void OnFinish(StatusEffect statusEffect)
                    {
                        Finished?.Invoke(statusEffect);
                        _methodIndex = -1;
                        _methodIndexOld = -1;
                    }
                }
            }
        }
#endif
    }
}