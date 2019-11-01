using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The welcome interface.
/// </summary>
public class Lobby : MonoBehaviour
{
    public List<CustomAnimRect> shopAnim;
    public List<Button> shopButtons;

    public List<CustomAnimRect> subTabs;
    
    public void Init()
    {
        foreach (var item in subTabs)
        {
            item.Teleport(0);
        }
    }

    public void SelectSubTab(int i)
    {
        for (int j = 0; j < subTabs.Count; j++)
        {
            if (j == i - 1)
            {
                subTabs[j].GoTo(1);
            }
            else
            {
                subTabs[j].GoTo(0);
            }
        }
    }

    public void SetShopTab(int i)
    {
        for (int j = 0; j < shopAnim.Count; j++)
        {
            if (j == i - 1)
            {
                shopAnim[j].GoTo(1);
            }
            else
            {
                shopAnim[j].GoTo(0);
            }
            shopButtons[j].interactable = (j != i - 1);
        }
    }
}
