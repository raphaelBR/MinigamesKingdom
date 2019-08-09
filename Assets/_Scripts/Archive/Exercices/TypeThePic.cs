using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeThePic : Exercice {

    public Image image;
    public InputField field;

    void OnEnable()
    {
        field.text = "";
        image.sprite = Dico.Picture(solution);
    }

    public void Validate()
    {
        Confirm(field.text == Dico.Foreign(solution));
    }
}
