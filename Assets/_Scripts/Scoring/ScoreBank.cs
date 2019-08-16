using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// Refreshes the generic in-game UI, such as the upper bar and the end screen.
/// </summary>
[RequireComponent(typeof(Animator))]
public class ScoreBank : MonoBehaviour
{
    [Header("System")]
    public bool firstFailPenalty;
    public int maxMultiplier = 4;
    public bool usesLife;
    [Range(1, 10)]
    public int maxLife = 3;
    public int pointsPerWin = 10;
    public int xpPerWin = 10;
    public int pointsPerLose = 1;
    public int xpPerLose = 5;
    [Header("Points")]
    public Text pointsCounter;
    public ParticleSystem pointsGain;
    public ParticleSystem pointsLoss;
    public float pointsDuration = 0.5f;
    public AnimationCurve pointsCurve;
    [Header("Completion")]
    public Text completionPercent;
    public Image completionBar;
    public float completionDuration = 0.5f;
    public AnimationCurve completionCurve;
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

    Coroutine c;
    Coroutine p;

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
                    EndGame(false);
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
            if (p != null)
            {
                StopCoroutine(p);
            }
            points = Mathf.Max(0, value);
            p = StartCoroutine(ChangePoints());
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
    float completion = 0f;
    public float Completion
    {
        get
        {
            return completion;
        }
        set
        {
            if (c != null)
            {
                StopCoroutine(c);
            }
            completion = Mathf.Clamp(value, 0f, 1f);
            c = StartCoroutine(ChangeCompletion());
        }
    }

    Animator anim;
    Dictionary<string, int> wordsXp = new Dictionary<string, int>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        points = 0;
        pointsCounter.text = "0";
        completion = 0f;
        completionBar.fillAmount = 0f;
        completionPercent.text = "0%";
        Multiplier = 1;
        Life = maxLife;
        Completion = 0f;
        
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
        Points += pointsPerWin * Multiplier;
        ChangeXP(word, xpPerWin * Multiplier);
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
            Points -= pointsPerLose;
        }
        if (usesLife)
        {
            Life--;
        }
        Multiplier = 1;
        ChangeXP(word, -xpPerLose);
    }

    public void EndGame(bool win)
    {
        Debug.Log("Insert Win/Lose Here");
        anim.SetTrigger("On");
        var playerXp = 0;
        var words = new List<ExpBar>();
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

    IEnumerator ChangePoints()
    {
        int origin = int.Parse(pointsCounter.text);
        int travel = points - origin;
        for (float f = 0f; f < 1f; f += Time.deltaTime / pointsDuration)
        {
            pointsCounter.text = ((int)(origin + pointsCurve.Evaluate(f) * travel + 0.5f)).ToString();
            yield return null;
        }
    }

    IEnumerator ChangeCompletion()
    {
        var origin = completionBar.fillAmount;
        var travel = completion - origin;
        var current = 0f;
        for (float f = 0f; f < 1f; f += Time.deltaTime / completionDuration)
        {
            current = origin + wordCurve.Evaluate(f) * travel;
            completionBar.fillAmount = current;
            completionPercent.text = (int)(current * 100f + 0.5f) + "%";
            yield return null;
        }
    }

    IEnumerator Replace()
    {
        var start = wordContent.anchoredPosition;
        var end = Vector2.zero;
        for (float f = 0f; f < 1f; f += Time.deltaTime / wordDuration)
        {
            wordContent.anchoredPosition = Vector2.Lerp(start, end, wordCurve.Evaluate(f));
            yield return null;
        }
    }

}
