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

            //string material = "Water";
            //WebClient webClient = new WebClient();
            //var colors = webClient.DownloadString($"https://www.google.com/search?q=rgb+color+of+" + material)
            //    .Split(new string[] { "<a", "</a>" }, StringSplitOptions.RemoveEmptyEntries)
            //    .Where(str => new string[] { material, "#" }.All(s => str.Contains(s)))
            //    .Select(str => str.Substring(str.IndexOf('#') + 1, 6))
            //    .Where(str => str.All(c => c == '#' || c >= '0' && c <= '9' || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F'))
            //    .ToArray();
            //Console.WriteLine(string.Join("\n", colors));

        }
    }
#endif
}
