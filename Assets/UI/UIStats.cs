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

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<VerticalLayoutGroup>().enabled = false;
        _panels.SetActive(false);
        gameObject.SetActive(false);
    }

    public void PanelsSetActive(bool value) => _panels.SetActive(value);
}
