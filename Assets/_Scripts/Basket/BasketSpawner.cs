using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketSpawner : MonoBehaviour
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
    
    Basket current;
    Vector3 align;
    ScoreBank bank;

    private void Awake()
    {
        bank = FindObjectOfType<ScoreBank>();
        align = basketPool.position - basketSpawn.position;
        Dico.AddPack("food");
        current = Instantiate(basketPrefab, basketPool);
        current.Init(4, 1);
        current.onComplete.AddListener(ScreenComplete);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bank.EndGame();
        }
    }

    void ScreenComplete()
    {
        var pos = current.transform.position;
        current.transform.parent = basketPool;
        current.transform.position = pos;

        current = Instantiate(basketPrefab, basketSpawn.position, basketSpawn.rotation, basketPool);
        current.Init(4, 1);
        current.onComplete.AddListener(ScreenComplete);
        StartCoroutine(Align());
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
