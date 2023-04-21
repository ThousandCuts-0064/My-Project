using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStats : MonoBehaviour
{
    private GameObject _panels;
    private BarList _embedded;
    private BarList _internals;
    private BarList _externals;
    public IReadOnlyStats Stats { get; set; }

    private void Awake()
    {
        _panels = transform.Find("Panels").gameObject;
        _embedded = transform.Find(nameof(Stats.Embedded)).GetComponent<BarList>();
        _internals = transform.Find(nameof(Stats.Internals)).GetComponent<BarList>();
        _externals = transform.Find(nameof(Stats.Externals)).GetComponent<BarList>();
    }

    private void Update()
    {
        UpdateResourceBars(_embedded, Stats.Embedded);
        UpdateResourceBars(_internals, Stats.Internals);
        UpdateResourceBars(_externals, Stats.Externals);

        static void UpdateResourceBars(BarList bars, IReadOnlyList<IReadOnlyResource> resources)
        {
            bars.Count = resources.Count;
            for (int i = 0; i < resources.Count; i++)
            {
                IReadOnlyResource resource = resources[i];
                Bar bar = bars[i];
                bar.Value = resource.Current / resource.Max;
                bar.FillColor = resource.Element.GetColor();
            }
        }
    }

    public void PanelsSetVisible(bool value) => _panels.SetActive(value);
}
