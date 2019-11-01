using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice : MonoBehaviour
{
    protected ProblemGenerator generator;
    protected ScoreBank bank;
    [HideInInspector]
    public Camera mainCam;
    
    protected void Awake()
    {
        mainCam = Camera.main;
        bank = FindObjectOfType<ScoreBank>();
        bank.target = this;
        generator = new ProblemGenerator();
    }

    virtual public bool UseJoker()
    {
        return true;
    }

}
