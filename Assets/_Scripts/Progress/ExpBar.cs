using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ExpBar : MonoBehaviour
{
    public Image icon;
    public Text foreignBar;
    public Text localeBar;
    [Header("Fill")]
    public float fillDuration;
    public AnimationCurve fillAnim;
    [Header("Xp")]
    public Text xpCount;
    public Image xpBar;
    public Image xpGainBar;
    public Text xpGainCount;
    [Header("Level Up")]
    public float levelupDuration = 0.25f;
    public ParticleSystem levelupParticles;
    public Text levelCount;
    public Text levelGainCount;

    Animator anim;
    int level;
    int xp;
    int xpCap;
    int xpGain;
    string key = "";

    public void Init (KeyValuePair<string, int> pair)
    {
        key = pair.Key;
        icon.sprite = Dico.Picture(pair.Key);
        foreignBar.text = Dico.Foreign(pair.Key);
        localeBar.text = Dico.Locale(pair.Key);
        xpGain = pair.Value;

        Experience exp = new Experience();
        if (Progress.progress.wordsMastery.TryGetValue(pair.Key, out exp) == false)
        {
            Progress.progress.wordsMastery.Add(pair.Key, exp);
        }
        xp = exp.Xp;
        level = exp.Level;

        xpCap = Progress.GetCap(pair.Key);
        Progress.GainXP(xpGain, pair.Key);
        Init();
    }

    public void Init(int value)
    {
        xpGain = value;
        Experience exp = Progress.GetExp();
        level = exp.Level;
        xp = exp.Xp;
        xpCap = Progress.GetCap();
        Progress.GainXP(xpGain);
        Init();
    }

    void Init()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("lvl", level);
        xpBar.fillAmount = (float)xp / xpCap;
        xpGainBar.fillAmount = xpBar.fillAmount;
        levelCount.text = level.ToString();
        xpCount.text = xp.ToString() + " / " + xpCap.ToString();
        levelGainCount.text = "";
        xpGainCount.text = "";
    }

    public IEnumerator Fill()
    {
        if (xpGain != 0)
        {
            xpGainCount.text = "+" + xpGain;
        }
        else
        {
            xpGainCount.text = "";
        }
        var startAmount = xpBar.fillAmount;
        var endAmount = startAmount + ((float)xpGain / xpCap);
        var startXp = xp;
        var endXp = xp + xpGain;
        var currentXp = 0;
        var levelGain = 0;
        if (xpGain != 0)
        {
            for (float f = 0f; f <= 1f; f += Time.deltaTime / fillDuration)
            {
                xpGainBar.fillAmount = Mathf.Lerp(startAmount, endAmount, fillAnim.Evaluate(f));
                currentXp = (int)(Mathf.Lerp(startXp, endXp, fillAnim.Evaluate(f)) + 0.5f);
                xpCount.text = currentXp.ToString() + " / " + xpCap.ToString();
                if (currentXp >= xpCap)
                {
                    level++;
                    levelCount.text = level.ToString();
                    levelGain++;
                    levelGainCount.text = "+" + levelGain;
                    xpCount.text = xpCap.ToString() + " / " + xpCap.ToString();
                    anim.SetInteger("lvl", level);
                    xpGainBar.fillAmount = 1f;
                    levelupParticles.Play();
                    yield return new WaitForSeconds(levelupDuration);
                    xpGain -= (xpCap - xp);
                    xp = 0;
                    xpBar.fillAmount = 0f;
                    if (key != "")
                    {
                        xpCap = Progress.GetCap(key);
                    }
                    else
                    {
                        xpCap = Progress.GetCap();
                    }
                    xpGainBar.fillAmount = 0f;
                    xpCount.text = "0 / " + xpCap.ToString();
                    yield return StartCoroutine(Fill()); 
                    break;
                }
                yield return null;
            }
        }
    }

}
