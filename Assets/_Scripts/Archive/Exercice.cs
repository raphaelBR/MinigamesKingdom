using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Exercice : MonoBehaviour
{
    public UnityEvent OnWin;
    public UnityEvent OnLose;

    protected string solution;
    protected string answer;
    
    protected void Awake()
    {
        Dico.AddPack("food");
        DebugBoot();
    }

    public virtual void DebugBoot()
    {
        Debug.Log("Test Sequence");
        Init(Dico.GetRandomKey());
    }

    public virtual void Init(string test)
    {

    }

    public virtual void Confirm(bool b)
    {
        if (b)
        {
            StartCoroutine(Win());
        }
        else
        {
            StartCoroutine(Fail());
        }
    }

    protected virtual IEnumerator Win()
    {
        Debug.Log("Win");
        gameObject.SetActive(false);
        yield return null;
    }

    protected virtual IEnumerator Fail()
    {
        Debug.Log("Fail");
        gameObject.SetActive(false);
        yield return null;
    }
}
