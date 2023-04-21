using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class BarList : MonoBehaviour, IReadOnlyList<Bar>
{
    private List<Bar> _bars;

    public Bar this[int index] { get => _bars[index]; set => _bars[index] = value; }

    public int Count
    {
        get => _bars.Count;
        set
        {
            if (_bars.Count == value)
                return;

            if (_bars.Count > value)
                for (int i = _bars.Count - 1; i >= value; i--)
                {
                    Destroy(_bars[i].gameObject);
                    _bars.RemoveAt(i);
                }
            else
                for (int i = _bars.Count; i < value; i++)
                    _bars.Add(Instantiate(UIPrefabs.Bar, transform));
        }
    }

    private void Awake()
    {
        _bars = new List<Bar>();
    }

    public List<Bar>.Enumerator GetEnumerator() => _bars.GetEnumerator();
    IEnumerator<Bar> IEnumerable<Bar>.GetEnumerator() => _bars.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _bars.GetEnumerator();
}
