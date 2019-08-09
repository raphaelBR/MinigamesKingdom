using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslateOut : Exercice {

    public Text text;
    public InputField field;
    
    void OnEnable()
    {
        field.text = "";
        text.text = Dico.Locale(solution);
    }

    public void Validate()
    {
        Confirm(field.text == Dico.Foreign(solution));
    }
}
