using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordscapeQuestion : MonoBehaviour
{
    public Text txt;
    public Image img;
    public CustomAnimRect anim;

    public void Init(string key)
    {
        img.sprite = null;
        txt.text = Dico.Locale(key);
        img.sprite = Dico.Picture(key);
    }

}
