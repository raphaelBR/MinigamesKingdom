using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Selects which question to ask in the minigames.
/// </summary>
public class ProblemGenerator
{

    public void Init()
    {
        Dico.AddPack("food");
        Debug.Log("Debug dictionary here");
    }

    public List<string> Generate(int i)
    {
        return Dico.GetRandomKeys(i);
    }

}
