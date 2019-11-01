using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssembleSpawner : Practice
{
    public Assemble assemblePrefab;
    public RectTransform assembleParent;
    public AssembleItem letterPrefab;
    public RectTransform letterParent;
    public RectTransform dumpPrefab;
    public RectTransform lettersDump;
    public RectTransform lettersSpawn;

    [Range(1, 3)]
    public int words = 2;
    [HideInInspector]
    public AssembleItem selected;

    List<Assemble> assembles = new List<Assemble>();
    List<AssembleItem> items = new List<AssembleItem>();

    float progress;
    float total;

    void Start()
    {
        generator.Init();
        List<string> keys = generator.Generate(words);
        List<RectTransform> dumps = new List<RectTransform>();

        int c = 0;
        for (int i = 0; i < words; i++)
        {
            assembles.Add(Instantiate(assemblePrefab, assembleParent));
            assembles[i].Init(keys[i], i, words, this);

            string foreign = Dico.Foreign(keys[i]).ToUpperInvariant();
            for (int j = 0; j < foreign.Length; j++)
            {
                items.Add(Instantiate(letterPrefab, letterParent));
                items[c].Init(foreign.Substring(j, 1), this);
                dumps.Add(Instantiate(dumpPrefab, lettersDump));
                c++;
            }
        }
        dumps.Shuffle();
        for (int k = 0; k < dumps.Count; k++)
        {
            items[k].anim.state[0].status = lettersSpawn;
            items[k].anim.state[1].status = dumps[k];
            items[k].root = dumps[k];
            items[k].anim.Play(0, 1);
        }
        progress = 0f;
        total = c;
    }

    public void LoseLife()
    {
        bank.Life--;
    }

    public void Progress()
    {
        progress++;
        bank.Completion = progress / total;
    }

    public void WordAchieved(string key)
    {
        bank.Success(key);
    }
}
