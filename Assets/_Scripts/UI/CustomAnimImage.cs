using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AnimStatusImg
{
    public string name;
    public Color status;
    public AnimationCurve anim = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float duration = 0.25f;
    public float delay = 0f;
}

/// <summary>
/// Translates between several RectTransforms without the use of an Animator.
/// </summary>
[RequireComponent(typeof(Image))]
public class CustomAnimImage : MonoBehaviour
{
    public List<AnimStatusImg> state;
    int current = 0;

    Image img;
    Image Img
    {
        get
        {
            if (img == null)
            {
                img = GetComponent<Image>();
            }
            return img;
        }
    }
    Coroutine adjust;
    Coroutine chain;
    float f = 0f;

    void Awake()
    {
        Teleport(current);
        adjust = null;
    }

    public void GoTo(int b)
    {
        Play(current, b);
    }

    public void PlayChain()
    {
        if (chain != null)
        {
            StopCoroutine(chain);
        }
        chain = StartCoroutine(Chain());
    }

    IEnumerator Chain()
    {
        for (int i = 0; i < state.Count - 1; i++)
        {
            Play(i, i + 1);
            yield return adjust;
        }
        chain = null;
    }

    public void Play(int a, int b)
    {
        if (a != b)
        {
            if (adjust != null)
            {
                StopCoroutine(adjust);
                f = 1 - f;
            }
            adjust = StartCoroutine(Transition(a, b));
            current = b;
        }
    }

    public IEnumerator Transition(int a, int b)
    {
        AnimStatusImg origin = state[a];
        AnimStatusImg target = state[b];
        yield return new WaitForSeconds(target.delay);
        while (f < 1f)
        {
            LerpImg(origin.status, target.status, target.anim, f);
            f += Time.deltaTime / target.duration;
            yield return null;
        }
        adjust = null;
        f = 0f;
        CopyImg(target.status);
    }

    void LerpImg(Color c1, Color c2, AnimationCurve c, float f)
    {
        if (c1 != null && c2 != null)
        {
            Img.color = Color.LerpUnclamped(c1, c2, c.Evaluate(f));
        }
    }

    public void Teleport(int i)
    {
        CopyImg(state[i].status);
        current = i;
    }

    void CopyImg(Color c)
    {
        if (c != null)
        {
            Img.color = c;
        }
    }

}
