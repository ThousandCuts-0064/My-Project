using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MoreLinq
{
    public static TSource MaxBy<TSource, TComparable>(this IEnumerable<TSource> source, Func<TSource, TComparable> selector) where TComparable : IComparable<TComparable>
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) throw new InvalidOperationException("Collection was empty");

        TSource maxItem = enumerator.Current;
        TComparable max = selector(maxItem);
        while (enumerator.MoveNext())
        {
            TSource currItem = enumerator.Current;
            TComparable curr = selector(currItem);
            if (curr.CompareTo(max) <= 0) continue;

            max = curr;
            maxItem = currItem;
        }
        return maxItem;
    }

    public static TSource MinBy<TSource, TComparable>(this IEnumerable<TSource> source, Func<TSource, TComparable> selector) where TComparable : IComparable<TComparable>
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (selector is null) throw new ArgumentNullException(nameof(selector));

        var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) throw new InvalidOperationException("Collection was empty");

        TSource minItem = enumerator.Current;
        TComparable min = selector(minItem);
        while (enumerator.MoveNext())
        {
            TSource currItem = enumerator.Current;
            TComparable curr = selector(currItem);
            if (curr.CompareTo(min) >= 0) continue;

            min = curr;
            minItem = currItem;
        }
        return minItem;
    }
}
