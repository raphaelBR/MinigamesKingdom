using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The interface used to unlocks packs and cards.
/// </summary>
public class Unlocker : MonoBehaviour
{
    public Transform packParent;
    public GameObject packPrefab;
    public Transform cardParent;
    public GameObject cardPrefab;

    public Text keysCount;

    private void Start()
    {
        
    }

    public void Init()
    {

    }

}
