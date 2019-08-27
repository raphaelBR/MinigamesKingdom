using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickerItem : MonoBehaviour
{
    public CustomAnimRect anim;
    public Text instruction;

    [HideInInspector]
    public string key;

    public void Init(string k, RectTransform a, RectTransform b)
    {
        anim.state[0].status = a;
        anim.state[1].status = b;
        anim.Teleport(0);
        instruction.text = Dico.Foreign(k);
        key = k;
    }

    public void Win()
    {
        anim.GoTo(1);
    }

}
