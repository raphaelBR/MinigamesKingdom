using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class ScoreBank : MonoBehaviour
{
    public Text pointsCounter;
    public ParticleSystem pointsGain;
    public ParticleSystem pointsLoss;
    public int maxMultiplier = 4;
    public bool firstFailPenalty;
    [Header("Health")]
    public bool usesLife;
    [Range(1, 10)]
    public int maxLife = 3;
    [Header("EndGame")]
    [Range(0f, 5f)]
    public float waitBeforeEnd = 1f;
    public ExpBar wordBar;
    public ExpBar playerBar;
    public Transform wordProgressContent;
    public RectTransform wordContent;
    public float wordAmount = 60f;
    public float wordDuration;
    public AnimationCurve wordCurve;
    public PointsTransfer pointsTransfer;

    int life = 3;
    public int Life
    {
        get
        {
            return life;
        }
        set
        {
            life = Mathf.Clamp(value, 0, maxLife);
            if (usesLife)
            {
                anim.SetInteger("Life", life);
                if (life == 0)
                {
                    EndGame();
                }
            }

        }
    }
    int points = 0;
    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = Mathf.Max(0, value);
            pointsCounter.text = points.ToString();
        }
    }
    int multiplier = 1;
    public int Multiplier
    {
        get
        {
            return multiplier;
        }
        set
        {
            multiplier = Mathf.Clamp(value, 1, maxMultiplier);
            anim.SetInteger("Multiplier", multiplier);
        }
    }

    Animator anim;
    Dictionary<string, int> wordsXp = new Dictionary<string, int>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        Points = 0;
        Multiplier = 1;
        Life = maxLife;
    }

    public void ChangeXP(string word, int amount)
    {
        if (wordsXp.ContainsKey(word) == false)
        {
            wordsXp.Add(word, 0);
        }
        wordsXp[word] = Mathf.Max(0, wordsXp[word] + amount);
    }


    public void Success(string word)
    {
        Points += 1 * Multiplier;
        ChangeXP(word, 1 * Multiplier);
        Multiplier++;
        pointsGain.Play();
    }

    public void Failure(string word)
    {
        if (firstFailPenalty == false || Multiplier == 1)
        {
            if (Points >= 1)
            {
                pointsLoss.Play();
            }
            Points -= 1;
        }
        if (usesLife)
        {
            Life--;
        }
        Multiplier = 1;
        ChangeXP(word, -1);
    }

    public void EndGame()
    {
        int playerXp = 0;
        anim.SetTrigger("On");
        List<ExpBar> words = new List<ExpBar>();
        foreach (var pair in wordsXp.OrderByDescending(key => key.Value))
        {
            Progress.GainXP(pair.Value, pair.Key);
            playerXp += pair.Value;
            var w = Instantiate(wordBar, wordProgressContent);
            w.Init(pair);
            words.Add(w);
        }
        Progress.GainXP(playerXp);
        playerBar.Init(playerXp);
        pointsTransfer.Init(Points);
        StartCoroutine(EndBars(words));
        Progress.Save();
    }

    IEnumerator EndBars(List<ExpBar> wordBars)
    {
        yield return new WaitForSeconds(waitBeforeEnd);
        anim.SetTrigger("End");
        Coroutine center = null;
        foreach (var word in wordBars)
        {
            yield return StartCoroutine(word.Fill());
            center = StartCoroutine(Center());
        }
        if (center != null)
        {
            StopCoroutine(center);
        }
        StartCoroutine(Replace());
        yield return StartCoroutine(playerBar.Fill());
        pointsTransfer.Transfer();
    }

    IEnumerator Center()
    {
        var start = wordContent.anchoredPosition;
        var end = start + new Vector2(0f, wordAmount);
        for (float f = 0; f < 1f; f+= Time.deltaTime / wordDuration)
        {
            wordContent.anchoredPosition = Vector2.Lerp(start, end, wordCurve.Evaluate(f));
            yield return null;
        }
    }

    IEnumerator Replace()
    {

        var start = wordContent.anchoredPosition;
        var end = Vector2.zero;
        for (float f = 0; f < 1f; f += Time.deltaTime / wordDuration)
        {
            wordContent.anchoredPosition = Vector2.Lerp(start, end, wordCurve.Evaluate(f));
            yield return null;
        }
    }

}
