using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {

    public Image img;
    public Text txt;
    bool good;
    Exercices manager;

    public void Setup(Exercices m, bool v, object o)
    {
        good = v;
        manager = m;
        if (o is Sprite)
        {
            img.sprite = (Sprite)o;
        }
        else if (o is string)
        {
            txt.text = (string)o;
        }
    }

    public void Confirm()
    {
        manager.Confirm(good);
    }
}
