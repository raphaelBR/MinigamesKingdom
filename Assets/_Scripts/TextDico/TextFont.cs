using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextFont : MonoBehaviour {
    
    protected Text t;
    protected Text Tx
    {
        get
        {
            if (t == null)
            {
                t = GetComponent<Text>();
            }
            return t;
        }
        set
        {
            t = value;
        }
    }

    void Start()
    {
        RefreshFont();
    }

    public void RefreshFont()
    {
        Tx.font = Parameters.Font;
    }
}
