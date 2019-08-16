using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatically translates the text’s content in the selected localized string, and that uses the custom font.
/// </summary>
[RequireComponent(typeof(Text))]
public class TextLocalized : TextFont {

    public enum Origin
    {
        Locale,
        Foreign
    }
    public Origin type;
    public string key;
    public bool menu = true;
    string idk;

    void Start()
    {
        idk = "[" + Tx.text + "]";
        RefreshText();
        RefreshFont();
    }

    public void RefreshText()
    {
        string t = Tx.text;
        switch (type)
        {
            case Origin.Locale:
                t = Dico.Locale(key, menu);
                break;
            case Origin.Foreign:
                t = Dico.Foreign(key, menu);
                break;
            default:
                break;
        }
        if (t == null)
        {
            t = idk;
        }
        Tx.text = t;
    }
}
