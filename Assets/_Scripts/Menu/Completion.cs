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
    public Text coins1Counter;
    public Text coins2Counter;
    public Text coins3Counter;

    public void Setup()
    {
        playerExp.Init(0);
        flag.sprite = Resources.Load<Sprite>("Flags/" + Parameters.Foreign.ToString());
        coins1Counter.text = Progress.progress.pointsA.ToString();
        coins2Counter.text = Progress.progress.pointsB.ToString();
        coins3Counter.text = Progress.progress.pointsC.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
