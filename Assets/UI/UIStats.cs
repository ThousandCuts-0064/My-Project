using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStats : MonoBehaviour
{
    private GameObject _panels;
    public IReadOnlyStats Stats { get; set; }

    private void Awake()
    {
        _panels = transform.Find("Panels").gameObject;
    }

    private void Start()
    {
        GetComponent<VerticalLayoutGroup>();
        //_panels.SetActive(false);
    }

    public void PanelsSetVisible(bool value) => _panels.SetActive(value);
}
