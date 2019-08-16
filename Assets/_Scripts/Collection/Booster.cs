using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pack of cards used in Minigames.
/// </summary>
public class Booster : MonoBehaviour
{

    public Text packName;

    [HideInInspector]
    public Toggle toggle;

    public void Init(string key)
    {
        toggle = GetComponent<Toggle>();
        packName.text = Dico.FirstCharToUpper(key);
    }

}
