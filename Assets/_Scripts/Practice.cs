using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns the game items and defines the game status.
/// </summary>
public class Practice : MonoBehaviour
{
    public ProblemGenerator generator;

    protected ScoreBank bank;

    protected void Awake()
    {
        bank = FindObjectOfType<ScoreBank>();
    }

    public void Win()
    {

    }

    public void Lose()
    {

    }

}
