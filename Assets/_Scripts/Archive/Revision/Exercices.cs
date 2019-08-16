using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public enum QuestionType
{
    Audio,
    Foreign,
    Image,
    Locale
}

public enum AnswerType
{
    ForeignField,
    Foreigns,
    LocaleField,
    Locales,
    Images
}

public class Exercices : MonoBehaviour   {

    public QuestionType question;
    public Text questionText;
    public Image questionImage;
    public AudioSource questionAudio;

    public AnswerType answer;
    public InputField answerField;
    public AnswerButton[] answerButtons;

    public Card flashcard;
    string solution;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Init(string test)
    {
        solution = test;
        switch (question)
        {
            case QuestionType.Audio:
                questionAudio.clip = Dico.Audio(solution);
                break;
            case QuestionType.Foreign:
                questionText.text = Dico.Foreign(solution);
                break;
            case QuestionType.Image:
                questionImage.sprite = Dico.Picture(solution);
                break;
            case QuestionType.Locale:
                questionText.text = Dico.Locale(solution);
                break;
            default:
                break;
        }
        switch (answer)
        {
            case AnswerType.ForeignField:
                answerField.text = "";
                break;
            case AnswerType.Foreigns:
                ButtonSetup();
                break;
            case AnswerType.LocaleField:
                answerField.text = "";
                break;
            case AnswerType.Locales:
                ButtonSetup();
                break;
            case AnswerType.Images:
                ButtonSetup();
                break;
            default:
                break;
        }
        anim.SetBool("on", true);
    }

    void ButtonSetup()
    {
        int a = Random.Range(0, answerButtons.Length - 1);
        List<string> keys = Dico.GetRandomKeys(answerButtons.Length);
        keys.Remove(solution);

        int b = 0;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (a != i)
            {
                answerButtons[i].Setup(this, false, GetAnswer(keys[b]));
                b++;
            }
            else
            {
                answerButtons[i].Setup(this, true, GetAnswer(solution));
            }
        }
    }

    object GetAnswer(string s)
    {
        switch (answer)
        {
            case AnswerType.ForeignField:
                return Dico.Foreign(s);
            case AnswerType.Foreigns:
                return Dico.Foreign(s);
            case AnswerType.LocaleField:
                return Dico.Locale(s);
            case AnswerType.Locales:
                return Dico.Locale(s);
            case AnswerType.Images:
                return Dico.Picture(s);
            default:
                return null;
        }
    }

    public void Validate()
    {
        if (answer == AnswerType.ForeignField)
        {
            Confirm(answerField.text == Dico.Foreign(solution));
        }
        else if (answer == AnswerType.LocaleField)
        {
            Confirm(answerField.text == Dico.Locale(solution));
        }
    }

    public void Confirm(bool b)
    {
        if (b)
        {
            Win();
        }
        else
        {
            Fail();
        }
        anim.SetBool("on", false);
    }

    void Win()
    {

    }

    void Fail()
    {
        //flashcard.Init(solution);
    }
}
