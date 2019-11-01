using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class WordscapeLetter : MonoBehaviour
{
    public Text t;
    public bool Interactable
    {
        get
        {
            return interactable;
        }
        set
        {
            interactable = value;
            anim.SetBool("Active", interactable);
        }
    }
    public CustomAnimRect animSpawn;
    [Header("AnimWin")]
    public float winDuration = 0.5f;
    public float circleSpeed = 360f;
    public AnimationCurve attract;
    public AnimationCurve spin;
    [Header("AnimWin")]
    public float loseDuration = 0.2f;
    public float shakeSpeed = 5f;
    public float shakeAmount = 15f;

    Wordscape master;
    private Animator anim;
    private bool interactable;

    public void Init(Wordscape dad, char c)
    {
        anim = GetComponent<Animator>();
        master = dad;
        t.text = c.ToString().ToUpper();
        Interactable = true;
    }

    public void Trigger()
    {
        if (interactable && Input.GetMouseButton(0))
        {
            master.InputLetter(t.text, transform.position);
            Interactable = false;
        }
    }

    public IEnumerator Fuse()
    {
        anim.SetTrigger("Win");
        for (float f = 0f; f < 1f; f += Time.deltaTime / winDuration)
        {
            transform.localPosition = Quaternion.Euler(0f, 0f, circleSpeed * spin.Evaluate(f)) * transform.localPosition;
            transform.localPosition = transform.localPosition.normalized * master.spawnRange * attract.Evaluate(f);
            yield return null;
        }
    }

    public IEnumerator Shake()
    {
        anim.SetTrigger("Fail");
        var root = transform.localPosition;
        for (float f = 0f; f < 1f; f += Time.deltaTime / loseDuration)
        {
            transform.localPosition = root + new Vector3(Mathf.Sin(Time.time * shakeSpeed) * shakeAmount, 0f, 0f);
            yield return null;
        }
        Interactable = true;
    }

}
