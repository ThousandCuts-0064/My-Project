using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Drawing;
using System.Net;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;
using Color = UnityEngine.Color;
using static Element;
using static ElementType;
using System.Linq;
using System.Globalization;

public static class ElementExt
{
    private static readonly Element[] _elements = ((Element[])Enum.GetValues(typeof(Element))).Where(e => e!= Element.None).ToArray();
    private static readonly string _elementColorsPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Elements", "ElemensColor.json");
    private static readonly Color _nullColor = new Color32(System.Drawing.Color.HotPink.R, System.Drawing.Color.HotPink.G, System.Drawing.Color.HotPink.B, byte.MaxValue);
    private static readonly Dictionary<Element, Color> _elementToColor;
    private static readonly IReadOnlyDictionary<ElementCombo, Element> _combinationTable = new Dictionary<ElementCombo, Element>()
    {
        { (Earth , Air  ), Sand      },
        { (Earth , Water), Plant     },
        { (Earth , Fire ), Sand      },
        { (Water , Air  ), Ice       },
        { (Fire  , Air  ), Lightning },
    };

    static ElementExt()
    {
        _elementToColor = new(Enum.GetNames(typeof(Element)).Length);
        if (!File.Exists(_elementColorsPath)) return;

        var colors = (Dictionary<Element, string>)JsonConvert.DeserializeObject(File.ReadAllText(_elementColorsPath), typeof(Dictionary<Element, string>));
        foreach (var pair in colors)
            _elementToColor.Add(pair.Key, ColorFromHex(pair.Value));
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    private static void Initialize() => GameManager.MakeButton("Update Elements Color", () =>
    {
        Dictionary<Element, string> elementToHexColor = File.Exists(_elementColorsPath)
            ? JsonConvert.DeserializeObject<Dictionary<Element, string>>(File.ReadAllText(_elementColorsPath))
            : new(Enum.GetNames(typeof(Element)).Length);

        foreach (var element in _elements)
        {
            if (elementToHexColor.TryGetValue(element, out string colorHex))
            {
                _elementToColor[element] = ColorFromHex(colorHex);
                continue;
            }

            string material = element.ToString();
            colorHex = new WebClient().DownloadString($"https://www.google.com/search?q=" + material + "+rgb+color")
                .Split(new string[] { "<a", "</a>" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(str => new string[] { material, "#" }.All(s => str.Contains(s)))
                .Select(str => str.Substring(str.IndexOf('#') + 1, 6))
                .Where(str => str.All(c => c == '#' || c >= '0' && c <= '9' || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F'))
                .Skip(1)
                .First();

            if (colorHex == null) continue;

            _elementToColor[element] = ColorFromHex(colorHex);
            elementToHexColor.Add(element, colorHex);
        }

        File.WriteAllText(_elementColorsPath, JsonConvert.SerializeObject(elementToHexColor, Formatting.Indented));
    });
#endif

    public static bool TryCombine(this Element e1, Element e2, out Element eOut) => _combinationTable.TryGetValue((e1, e2), out eOut);

    public static Color GetColor(this Element element)
    {
        if (!_elementToColor.TryGetValue(element, out Color color))
            color = _nullColor;
        return color;
    }

    public static ElementType GetState(this Element element) => element switch
    {
        Element.None => throw new InvalidEnumArgumentException(nameof(element), (int)element, typeof(Element)),

        Water => Liquid,
        Air => Gas,
        Stone => Solid,
        Fire => Plasma,

        _ => Enum.IsDefined(typeof(Element), element)
            ? throw new NotImplementedException()
            : throw new InvalidEnumArgumentException(nameof(element), (int)element, typeof(Element)),
    };

    private static Color ColorFromHex(string hex) => new Color32(
        byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
        byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
        byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber),
        byte.MaxValue);

    private static string ColorToHex(Color32 color) => 
        color.r.ToString("X").PadLeft(2, '0') + 
        color.g.ToString("X").PadLeft(2, '0') + 
        color.b.ToString("X").PadLeft(2, '0');

    private readonly struct ElementCombo : IEquatable<ElementCombo>
    {
        public readonly Element E1;
        public readonly Element E2;

        public ElementCombo(Element e1, Element e2)
        {
            E1 = e1;
            E2 = e2;
        }

        public bool Equals(ElementCombo other) => other == this;
        public override bool Equals(object obj) => obj is ElementCombo ec && ec == this;
        public override int GetHashCode() => (int)(E1 | E2);
        public override string ToString() => $"{{{E1}, {E2}}}";


        public static implicit operator ElementCombo((Element e1, Element e2) elements) => new(elements.e1, elements.e2);

        public static bool operator ==(ElementCombo l, ElementCombo r) =>
            l.E1 == r.E1 && l.E2 == r.E2 ||
            l.E1 == r.E2 && l.E2 == r.E2;

        public static bool operator !=(ElementCombo l, ElementCombo r) => !(l == r);
    }
}
