using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextUtils
{
    #region System de couleur dans un texte.
    public static string GetColorToString(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static string GetColorText(string myString, Color32 myColor)
    {
        return "<color=#" + GetColorToString(myColor) + ">" + myString + "<" + "/color>";
    }
    #endregion
}
