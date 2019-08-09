using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wordscape : Exercice
{
    [Header("Question")]
    public Image questionImage;
    public Text questionText;
    [Header("Input")]
    public Text answerText;
    public WordscapeLetter letterPrefab;
    public float spawnRange = 120f;
    public Transform spawnCenter;
    [Header("Feedback")]
    public LineRenderer line;
    public LineRenderer line2;
    public ParticleSystem charge;
    public ParticleSystem explode;
    
    List<WordscapeLetter> letters = new List<WordscapeLetter>();
    List<Vector3> linePoints = new List<Vector3>();

    public override void Init(string test)
    {
        // Find a word
        questionImage.sprite = Dico.Picture(test);
        questionText.text = Dico.Locale(test).ToUpper();
        solution = Dico.Foreign(test);
        // Clear the board
        answer = "";
        answerText.text = answer.PadRight(solution.Length, '\u005F').Replace("_", " _");
        foreach (WordscapeLetter l in letters)
        {
            Destroy(l.gameObject);
        }
        letters.Clear();
        linePoints.Clear();
        // Spawn letters
        foreach (char c in solution)
        {
            var o = Instantiate(letterPrefab, spawnCenter);
            o.Init(this, c);
            letters.Add(o);
        }
        // Place letters randomly
        Dico.Shuffle(letters);
        int i = 0;
        var angle = 360f / solution.Length;
        foreach (WordscapeLetter t in letters)
        {
            var x = spawnCenter.position.x + spawnRange * Mathf.Sin(angle * i * Mathf.Deg2Rad);
            var y = spawnCenter.position.y + spawnRange * Mathf.Cos(angle * i * Mathf.Deg2Rad);
            t.transform.localPosition = new Vector3(x, y);
            i++;
        }
    }

    public void InputLetter(string s, Vector3 pos)
    {
        answer = answer + s;
        answerText.text = answer.PadRight(solution.Length, '\u005F').Replace("_", " _");
        linePoints.Add(new Vector3(pos.x, pos.y, 0f));
        if (solution.Length == answer.Length)
        {
            linePoints.Clear();
            Confirm(string.Equals(solution, answer, System.StringComparison.OrdinalIgnoreCase));
        }
    }

    protected override IEnumerator Win()
    {
        foreach (WordscapeLetter t in letters)
        {
            StartCoroutine(t.Fuse());
            charge.Play();
        }
        yield return new WaitForSeconds(0.5f);
        explode.Play();
        yield return new WaitForSeconds(0.2f);
        OnWin.Invoke();
        Init(Dico.GetRandomKey());
    }

    protected override IEnumerator Fail()
    {
        foreach (WordscapeLetter t in letters)
        {
            StartCoroutine(t.Shake());
        }
        yield return new WaitForSeconds(0.5f);
        OnLose.Invoke();
        //Init(Dico.RandomKey());
    }

    public void Reset()
    {
        foreach (WordscapeLetter t in letters)
        {
            t.Interactable = true;
        }
        linePoints.Clear();
        answer = "";
        answerText.text = answer.PadRight(solution.Length, '\u005F').Replace("_", " _");
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && solution.Length != answer.Length)
        {
            Reset();
        }
        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.ToArray());
        if (linePoints.Count > 0)
        {
            line2.positionCount = 2;
            Vector3[] second = new Vector3[2];
            second[0] = linePoints[linePoints.Count - 1];
            second[1] = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0f);
            line2.SetPositions(second);
        }
        else
        {
            line2.SetPositions(linePoints.ToArray());
            line2.positionCount = 0;
        }
    }

}
