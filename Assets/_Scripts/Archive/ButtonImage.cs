using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour {

    public Image img;
    bool good;
    Exercice manager;

    public void Setup(Exercice m, bool v, Sprite s)
    {
        good = v;
        manager = m;
        img.sprite = s;
    }

    public void Confirm()
    {
        manager.Confirm(good);
    }

}
