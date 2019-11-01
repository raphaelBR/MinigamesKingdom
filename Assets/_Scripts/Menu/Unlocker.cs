using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UnlockType
{
    Pack,
    Skin,
    Font
}

/// <summary>
/// The interface used to unlocks packs and cards.
/// </summary>
public class Unlocker : MonoBehaviour
{
    public UnlockType type;
    public string id;
    public Button btn;

    [Header("Price")]
    public int cost;
    public Text costDisplay;
    public Color tooPricey;
    public Color purchasable;

    void Start()
    {
        Init();
        costDisplay.text = cost.ToString();
    }

    public void Init()
    {
        if (Account.IsUnlocked(type, id))
        {
            gameObject.SetActive(false);
        }
        else if (cost > Progress.Coins)
        {
            btn.interactable = false;
            costDisplay.color = tooPricey;
        }
        else
        {
            costDisplay.color = purchasable;
        }
    }

    public void Unlock()
    {
        Account.Unlock(type, id);
        Progress.Coins -= cost;
        gameObject.SetActive(false);
    }

}
