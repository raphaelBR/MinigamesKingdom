using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public Font currentFont = null;
    public Skin currentSkin = new Skin();
    public Languages locale = Languages.Null;
    public Languages foreign = Languages.Null;
}