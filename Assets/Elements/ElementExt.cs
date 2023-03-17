using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Drawing;
using Color = UnityEngine.Color;
using static Element;
using static ElementType;

public static class ElementExt
{
    private static readonly IReadOnlyDictionary<ElementCombo, Element> _combinationTable = new Dictionary<ElementCombo, Element>()
    {
        { (Earth , Air  ), Sand      },
        { (Earth , Water), Plant     },
        { (Earth , Fire ), Sand      },
        { (Water , Air  ), Ice       },
        { (Fire  , Air  ), Lightning },
    };

    public static bool TryCombine(this Element e1, Element e2, out Element eOut) => _combinationTable.TryGetValue((e1 , e2), out eOut);

    public static ElementType GetState(this Element element) => element switch
    {
        Element.None => throw new InvalidEnumArgumentException(nameof(element), (int)element, typeof(Element)),

        Water => Liquid,
        Air   => Gas,
        Stone => Solid,
        Fire  => Plasma,



        _ => Enum.IsDefined(typeof(Element), element)
            ? throw new NotImplementedException()
            : throw new InvalidEnumArgumentException(nameof(element), (int)element, typeof(Element)),
    };

    public static Color GetColor(this Element element) => ToColor(element switch
    {
        Element.None => throw new InvalidEnumArgumentException(nameof(element), (int)element, typeof(Element)),

        Water => KnownColor.Aqua,
        Air   => KnownColor.WhiteSmoke,
        Stone => KnownColor.Gray,
        Fire  => KnownColor.Orange,



        _ => Enum.IsDefined(typeof(Element), element) 
            ? throw new NotImplementedException() 
            : throw new InvalidEnumArgumentException(nameof(element), (int)element, typeof(Element)),
    });

    private static Color ToColor(KnownColor knownColor)
    {
        var color = System.Drawing.Color.FromKnownColor(knownColor);
        return new Color32(color.R, color.G, color.B, color.A);
    }

    private readonly struct ElementCombo
    {
        public readonly Element E1;
        public readonly Element E2;

        public ElementCombo(Element e1, Element e2)
        {
            E1 = e1;
            E2 = e2;
        }

        public override bool Equals(object obj) => obj is ElementCombo ec && ec == this;

        public override int GetHashCode() => (int)(E1 | E2);

        public override string ToString() => $"{{{E1}, {E2}}}";

        public static implicit operator ElementCombo((Element e1, Element e2) es) => new(es.e1, es.e2);

        public static bool operator ==(ElementCombo l, ElementCombo r) =>
            l.E1 == r.E1 && l.E2 == r.E2 ||
            l.E1 == r.E2 && l.E2 == r.E2;

        public static bool operator !=(ElementCombo l, ElementCombo r) => !(l == r);
    }
}
