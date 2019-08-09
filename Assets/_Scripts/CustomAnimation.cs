using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AnimStatus
{
    public string name;
    public RectTransform status;
    public AnimationCurve anim = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float duration = 0.25f;
    public float delay = 0f;
}

[RequireComponent(typeof(RectTransform))]
public class CustomAnimation : MonoBehaviour
{
    public string name;
    public List<AnimStatus> state;
    public int current = 0;

    RectTransform rect;
    RectTransform Rect
    {
        get
        {
            if (rect == null)
            {
                rect = GetComponent<RectTransform>();
            }
            return rect;
        }
    }
    Coroutine adjust;
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
        AnimStatus origin = state[a];
        AnimStatus target = state[b];
        yield return new WaitForSeconds(target.delay);
        while (f < 1f)
        {
            LerpRect(origin.status, target.status, target.anim, f);
            f += Time.deltaTime / target.duration;
            yield return null;
        }
        adjust = null;
        f = 0f;
        CopyRect(target.status);
        
    }

    void LerpRect(RectTransform r1, RectTransform r2, AnimationCurve c, float f)
    {
        if (r1 != null && r2 != null)
        {
            Rect.anchorMin = Vector2.Lerp(r1.anchorMin, r2.anchorMin, c.Evaluate(f));
            Rect.anchorMax = Vector2.Lerp(r1.anchorMax, r2.anchorMax, c.Evaluate(f));
            Rect.offsetMin = Vector2.Lerp(r1.offsetMin, r2.offsetMin, c.Evaluate(f));
            Rect.offsetMax = Vector2.Lerp(r1.offsetMax, r2.offsetMax, c.Evaluate(f));
            Rect.pivot = Vector2.Lerp(r1.pivot, r2.pivot, c.Evaluate(f));
            Rect.rotation = Quaternion.LerpUnclamped(r1.rotation, r2.rotation, c.Evaluate(f));
            Rect.localScale = Vector3.Lerp(r1.localScale, r2.localScale, c.Evaluate(f));
            Rect.position = Vector3.Lerp(r1.position, r2.position, c.Evaluate(f));
        }
    }

    public void Teleport(int i)
    {
        CopyRect(state[i].status);
        current = i;
    }

    void CopyRect(RectTransform r)
    {
        if (r != null)
        {
            Rect.anchorMin = r.anchorMin;
            Rect.anchorMax = r.anchorMax;
            Rect.offsetMin = r.offsetMin;
            Rect.offsetMax = r.offsetMax;
            Rect.pivot = r.pivot;
            Rect.rotation = r.rotation;
            Rect.localScale = r.localScale;
            Rect.position = r.position;
        }
    }

}
