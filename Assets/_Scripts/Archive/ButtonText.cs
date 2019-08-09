using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour {

    public Text text;
    bool good;
    Exercice manager;

    public void Setup(Exercice m, bool v, string s)
    {
        good = v;
        manager = m;
        text.text = s;
    }

    public void Confirm()
    {
        manager.Confirm(good);
    }

}
