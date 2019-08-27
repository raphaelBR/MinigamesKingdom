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
    int layers = 5;
    
    Basket current;
    Vector3 align;
    int layerCount = 0;

    void Start()
    {
        generator.Init();
        align = basketPool.position - basketSpawn.position;
        current = Instantiate(basketPrefab, basketPool);
        current.Init(generator.Generate(5), 1);
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
            current.Init(generator.Generate(5), 1);
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
    }
}
