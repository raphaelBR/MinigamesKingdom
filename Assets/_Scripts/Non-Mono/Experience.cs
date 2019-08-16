using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Experience
{
    [SerializeField]
    int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            if (value < 1)
            {
                level = 1;
            }
            else
            {
                level = value;
            }
        }
    }

    [SerializeField]
    int xp = 0;
    public int Xp
    {
        get
        {
            return xp;
        }
        set
        {
            if (value < 0)
            {
                xp = 0;
            }
            else
            {
                xp = value;
            }
        }
    }
}