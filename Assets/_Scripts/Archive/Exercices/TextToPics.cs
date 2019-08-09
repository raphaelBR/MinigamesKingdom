using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextToPics : Exercice {

    public Text text;
    public ButtonImage[] images;

    void OnEnable()
    {
        text.text = Dico.Foreign(solution);
        int a = Random.Range(0, images.Length - 1);
        List<string> keys = Dico.GetRandomKeys(images.Length);
        keys.Remove(solution);

        int b = 0;
        for (int i = 0; i < images.Length; i++)
        {
            if (a != i)
            {
                images[i].Setup(this, false, Dico.Picture(keys[b]));
                b++;
            }
            else
            {
                images[i].Setup(this, true, Dico.Picture(solution));
            }
        }
    }
}
