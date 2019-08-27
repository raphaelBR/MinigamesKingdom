using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice : MonoBehaviour
{
    protected ProblemGenerator generator;
    protected ScoreBank bank;
    
    protected void Awake()
    {
        bank = FindObjectOfType<ScoreBank>();
        generator = new ProblemGenerator();
    }

}
