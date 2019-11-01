using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assemble : MonoBehaviour
{
    public Image img;
    public Text locale;
    public AssembleSpot spotPrefab;
    public RectTransform horizontalLayout;
    public RectTransform rect;

    AssembleSpot[] spots;

    string answer;
    AssembleSpawner master;
    string k;

    public void Init(string key, int position, int total, AssembleSpawner m)
    {
        k = key;
        master = m;
        Vector2 oMin = rect.offsetMin;
        Vector2 oMax = rect.offsetMax;
        rect.anchorMin = new Vector2(0f, (float)position / total);
        rect.anchorMax = new Vector2(1f, (position + 1f) / total);
        rect.offsetMin = oMin;
        rect.offsetMax = oMax;
        answer = Dico.Foreign(key).ToUpperInvariant();
        img.sprite = Dico.Picture(key);
        locale.text = Dico.Locale(key);
        spots = new AssembleSpot[answer.Length];
        for (int i = 0; i < spots.Length; i++)
        {
            spots[i] = Instantiate(spotPrefab, horizontalLayout);
            spots[i].master = master;
            spots[i].group = this;
        }
    }

    public void Verify()
    {
        // Check if all spots have a letter
        foreach (var spot in spots)
        {
            if (spot.docked == null)
            {
                return;
            }
        }

        // Checks if all the letters are good
        bool test = true;
        for (int i = 0; i < answer.Length; i++)
        {
            if (answer.Substring(i, 1) != spots[i].docked.c)
            {
                spots[i].Eject();
                test = false;
            }
            else
            {
                spots[i].Lock();
            }
        }
        if (test)
        {
            master.WordAchieved(k);
        }
        else
        {
            Debug.Log("Fail");
            master.LoseLife();
        }
    }
}

