using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The interface that condenses the player progress.
/// </summary>
public class Completion : MonoBehaviour
{

    public ExpBar playerExp;
    public Image flag;
    public Text coinsCounter;

    public void Setup()
    {
        playerExp.Init(0);
        flag.sprite = Resources.Load<Sprite>("Flags/" + Parameters.Foreign.ToString());
        coinsCounter.text = Progress.Coins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
