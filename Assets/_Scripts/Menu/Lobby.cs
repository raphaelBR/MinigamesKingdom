using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The welcome interface.
/// </summary>
public class Lobby : MonoBehaviour
{
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
}
