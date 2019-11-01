using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Spawns the game items and defines the game status.
/// </summary>
public class BasketSpawner : Practice
{

    [Header("Exercise")]
    public Transform basketSpawn;
    public Transform basketPool;
    public Basket basketPrefab;
    [Header("Parallax")]
    public RectTransform parallax;
    public float parallaxAmount = 10f;
    [Header("Transition")]
    public float waitBeforeAlign = 0.5f;
    public float alignDuration = 0.5f;
    public AnimationCurve alignAnim;
    [Header("Spawning")]
    [Range(1, 15)]
    public int layers = 5;
    [Range(1, 8)]
    public int boxesPerLayer = 5;
    [Range(0, 3)]
    public int fakesPerLayer = 1;

    Basket current;
    Vector3 align;
    int layerCount = 0;
    int progress = 0;
    int total;

    void Start()
    {
        generator.Init();
        total = layers * boxesPerLayer;
        align = basketPool.position - basketSpawn.position;
        current = Instantiate(basketPrefab, basketPool);
        current.Init(generator.Generate(boxesPerLayer + fakesPerLayer), this, fakesPerLayer);
        current.onComplete.AddListener(ScreenComplete);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bank.EndGame(true);
        }
    }

    void ScreenComplete()
    {
        layerCount++;
        if (layerCount >= layers)
        {
            bank.EndGame(true);
        }
        else
        {
            var pos = current.transform.position;
            current.transform.parent = basketPool;
            current.transform.position = pos;

            current = Instantiate(basketPrefab, basketSpawn.position, basketSpawn.rotation, basketPool);
            current.Init(generator.Generate(boxesPerLayer + fakesPerLayer), this, fakesPerLayer);
            current.onComplete.AddListener(ScreenComplete);
            StartCoroutine(Align());
        }
    }

    IEnumerator Align()
    {
        yield return new WaitForSeconds(waitBeforeAlign);
        var start = basketPool.position;
        var end = start + align;
        var parallaxStart = parallax.anchoredPosition;
        var parallaxEnd = parallaxStart + Vector2.down * parallaxAmount;
        for (float f = 0f; f < 1; f += Time.deltaTime / alignDuration)
        {
            basketPool.position = Vector3.Lerp(start, end, alignAnim.Evaluate(f));
            parallax.anchoredPosition = Vector2.Lerp(parallaxStart, parallaxEnd, alignAnim.Evaluate(f));    
            yield return null;
        }
        bank.EnableJoker(current.fakes.Count > 0);
    }

    override public bool UseJoker()
    {
        return current.Joker();
    }

    public void Success(string k)
    {
        bank.Success(k);
        progress++;
        bank.Completion = (float)progress / total;
    }

    public void Failure(string k1, string k2)
    {
        bank.Failure(k1);
        bank.ChangeXP(k2, -1);
    }
}
