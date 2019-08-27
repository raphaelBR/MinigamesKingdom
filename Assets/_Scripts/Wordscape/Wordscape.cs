using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wordscape : Practice
{
    [Header("Question")]
    public WordscapeQuestion questionPrefab;
    public Transform questionParent;
    public RectTransform questionBefore;
    public RectTransform questionIn;
    public RectTransform questionAfter;
    [Header("Input")]
    public Text answerText;
    public WordscapeLetter letterPrefab;
    public RectTransform inputZone;
    public RectTransform inputField;
    public Transform spawnCenter;
    [Header("Feedback")]
    public LineRenderer line;
    public LineRenderer line2;
    public ParticleSystem charge;
    public ParticleSystem explode;
    public int total = 6;

    List<string> words = new List<string>();
    List<WordscapeLetter> letters = new List<WordscapeLetter>();
    List<Vector3> linePoints = new List<Vector3>();
    [HideInInspector]
    public float spawnRange = 100f;
    string solution;
    string answer;
    int progress;
    WordscapeQuestion current;

    private void Start()
    {
        // Scale
        questionIn.offsetMin = new Vector2(0f, Screen.width * 0.85f + 100f);
        inputField.offsetMin = new Vector2(0f, Screen.width * 0.85f);
        inputField.offsetMax = new Vector2(0f, Screen.width * 0.85f + 100f);
        inputZone.offsetMax = new Vector2(0f, Screen.width * 0.85f);
        spawnRange = Screen.width * 0.3f;
        // Init
        generator.Init();
        words = generator.Generate(total);
        progress = 0;
        Init();
    }

    public void Init()
    {
        // Find a word
        string test = words[progress];
        current = Instantiate(questionPrefab, questionParent);
        current.Init(test);
        current.anim.state[0].status = questionBefore;
        current.anim.state[1].status = questionIn;
        current.anim.state[2].status = questionAfter;
        current.anim.Play(0, 1);

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
            if (string.Equals(solution, answer, System.StringComparison.OrdinalIgnoreCase))
            {
                StartCoroutine(Win());
            }
            else
            {
                StartCoroutine(Fail());
            }
        }
    }

    IEnumerator Win()
    {
        foreach (WordscapeLetter t in letters)
        {
            StartCoroutine(t.Fuse());
            charge.Play();
        }
        yield return new WaitForSeconds(0.5f);
        explode.Play();
        yield return new WaitForSeconds(0.2f);
        bank.Success(words[progress]);
        current.anim.Play(1, 2);
        //OnWin.Invoke();
        progress++;
        bank.Completion = (float)progress / total;
        if (progress >= total)
        {
            bank.EndGame(true);
        }
        else
        {
            Init();
        }
    }

    IEnumerator Fail()
    {
        bank.Failure(words[progress]);
        foreach (WordscapeLetter t in letters)
        {
            StartCoroutine(t.Shake());
        }
        yield return new WaitForSeconds(0.5f);
        answer = "";
        answerText.text = answer.PadRight(solution.Length, '\u005F').Replace("_", " _");

        //OnLose.Invoke();
        //Init();
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
