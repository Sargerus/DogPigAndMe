using System;
using UnityEngine;

public static class ColorExtensions
{
    public static bool CompareColors(this Color one, UnityEngine.Color two)
    {
        bool compareResultR = false, compareResultG = false, compareResultB = false;

        float number = 0.04f;

        if (one.r.Equals(two.r) || Math.Abs(one.r - two.r) <= number)
            compareResultR = true;

        if (one.g.Equals(two.g) || Math.Abs(one.g - two.g) <= number)
            compareResultG = true;

        if (one.b.Equals(two.b) || Math.Abs(one.b - two.b) <= number)
            compareResultB = true;

        return compareResultR && compareResultG && compareResultB;
    } 

    public static bool ColorIsEmpty(this Color color)
    {
        bool result = false;

        //check if color is black -> empty
        if (color.r.Equals(0f) && color.g.Equals(0f) && color.b.Equals(0f))
            result = true;

        return result;
    }
}
