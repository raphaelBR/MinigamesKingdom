using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TranslateIn : Exercice {

    public Text text;
    public InputField field;

    void OnEnable()
    {
        field.text = "";
        text.text = Dico.Foreign(solution);
    }

    public void Validate()
    {
        Confirm(field.text == Dico.Locale(solution));
    }
}
