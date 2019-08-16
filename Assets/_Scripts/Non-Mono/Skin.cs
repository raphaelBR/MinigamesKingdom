using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skin
{
    public string theme = "default";
    public Color content = new Color(42f / 255f, 43f / 255f, 60f / 255f);
    public Color clickable = new Color(255f / 255f, 255f / 255f, 255f / 255f);
    public Color back = new Color(204f / 255f, 204f / 255f, 204f / 255f);
    public Color bad = new Color(245f / 255f, 72f / 255f, 72f / 255f);
    public Color good = new Color(186f / 255f, 245f / 255f, 72f / 255f);
    public Color highlight = new Color(72f / 255f, 186f / 255f, 245f / 255f);

}