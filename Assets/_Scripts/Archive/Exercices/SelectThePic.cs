using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectThePic : Exercice {
    
    public Image image;
    public ButtonText[] buttons;

    void OnEnable()
    {
        image.sprite = Dico.Picture(solution);
        int a = Random.Range(0, buttons.Length - 1);
        List<string> keys = Dico.GetRandomKeys(buttons.Length);
        keys.Remove(solution);

        int b = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (a != i)
            {
                buttons[i].Setup(this, false, Dico.Foreign(keys[b]));
                b++;
            }
            else
            {
                buttons[i].Setup(this, true, Dico.Foreign(solution));
            }
        }
    }
}
