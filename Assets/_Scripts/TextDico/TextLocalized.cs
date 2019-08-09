using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
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
            t = "[" + Tx.text + "]";
        }
        Tx.text = t;
    }
}
