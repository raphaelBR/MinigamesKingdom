using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The interface used to select a minigame.
/// </summary>
public class Minigames : MonoBehaviour
{
    [Header("Coins")]
    public Text pointsA;
    public Text pointsB;
    public Text pointsC;
    [Header("Costs")]
    public int costIntermediate;
    public int costExpert;
    [Header("Costs")]
    public Text boostCount;
    public Text multiplierCount;
    public Text multiplierCount2;
    [Header("Costs")]
    public ToggleBundle boosters;
    public Booster boosterPrefab;
    public Transform boosterParent;
    [Header("Anims")]
    public List<CustomAnimation> lexicalSwitch;
    public List<CustomAnimation> helpers;
    public CustomAnimation easyDoor;
    public CustomAnimation mediumDoor;
    public CustomAnimation hardDoor;

    List<string> packs = new List<string>();
    bool lex = false;

    public void Init()
    {
        lex = false;
        foreach (var item in lexicalSwitch)
        {
            item.Teleport(0);
        }
        foreach (var item in helpers)
        {
            item.Teleport(0);
        }

        pointsA.text = Progress.progress.pointsA.ToString();
        pointsB.text = Progress.progress.pointsB.ToString();
        pointsC.text = Progress.progress.pointsC.ToString();

        easyDoor.Teleport(0);
        easyDoor.Play(0, 1);

        mediumDoor.Teleport(0);
        if (Progress.progress.pointsA >= costIntermediate)
        {
            mediumDoor.Play(0, 1);
        }

        hardDoor.Teleport(0);
        if (Progress.progress.pointsB >= costExpert)
        {
            hardDoor.Play(0, 1);
        }

        foreach (var booster in boosters.members)
        {
            Destroy(booster.gameObject);
        }
        boosters.members.Clear();

        foreach (var item in Account.unlocks.packs)
        {
            var b = Instantiate(boosterPrefab, boosterParent);
            b.Init(item);
            boosters.members.Add(b.toggle);
            b.toggle.onValueChanged.AddListener(delegate { boosters.Filter(b.toggle); });
            b.GetComponent<Image>().sprite = Dico.GetPackage(item).booster;
            b.name = item;
        }
        boosters.Filter();
    }

    public void RefreshPacks()
    {
        int count = boosters.members.Count;

        if (boosters.any.isOn == false)
        {
            count = 0;
            foreach (var member in boosters.members)
            {
                if (member.isOn)
                {
                    count++;
                }
            }
        }

        boostCount.text = count.ToString() + " packs";
        if (count <= 1)
        {
            multiplierCount.text = "No bonus";
            multiplierCount2.text = "-";
        }
        else
        {
            multiplierCount.text = "Exp +" + ((count - 1) * 10).ToString() + "%";
            multiplierCount2.text = "+" + ((count - 1) * 10).ToString() + "%";
        }
    }

    public void FilterAny()
    {
        boosters.Filter();
    }

    public void SetHelper(int i)
    {
        if (i == 0)
        {
            foreach (var item in helpers)
            {
                item.GoTo(0);
            }
        }
        else
        {
            helpers[i - 1].GoTo(1);
        }
    }

    public void ToggleLexical()
    {
        lex = !lex;
        SetLexical(lex);
    }

    void SetLexical(bool b)
    {
        int i = 0;
        if (b)
        {
            i = 1;
        }
        foreach (var anim in lexicalSwitch)
        {
            anim.GoTo(i);
        }
    }

    public void Pay(int i)
    {
        if (i == 0)
        {
            Progress.progress.pointsA -= costIntermediate;
        }
        else
        {
            Progress.progress.pointsB -= costExpert;
        }
    }

    public void LoadDicos()
    {
        Dico.dico.Clear();
        if (boosters.any.isOn)
        {
            foreach (string s in Account.unlocks.packs)
            {
                Dico.AddPack(s);
            }
        }
        else
        {
            foreach (Toggle b in boosters.members)
            {
                if (b.isOn)
                {
                    Dico.AddPack(b.name);
                }
            }
        }
    }

}
