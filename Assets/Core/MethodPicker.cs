using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

abstract class MethodPicker<T>
{
    private static readonly MethodInfo[] _methods;
    private static readonly string[] _methodsSelfAndParamsName;

    private readonly string _name;

    private ParameterInfo[] _methodParamsInfo;
    private object[] _methodParams;
    private MethodInfo _methodInfo;
    private int _methodIndex = -1;
    private int _methodIndexOld = -1;

    static MethodPicker()
    {
        _methods = typeof(T)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .ToArray();

        _methodsSelfAndParamsName = _methods
            .Select(m => $"{m.Name}({string.Join(", ", m.GetParameters().Select(p => p.Name))})")
            .ToArray();
    }

    public MethodPicker(string name)
    {
        _name = name;
    }

    public virtual void OnInspectorGUI()
    {
        EditorStyles.label.fontStyle = FontStyle.Bold;
        _methodIndex = EditorGUILayout.Popup(_name, _methodIndex, _methodsSelfAndParamsName);
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
    }

    protected object Invoke(object obj) => _methodInfo.Invoke(obj, _methodParamsInfo);
}
