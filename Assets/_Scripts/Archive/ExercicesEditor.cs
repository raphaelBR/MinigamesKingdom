using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Exercices))]
[CanEditMultipleObjects]
public class ExercicesEditor : Editor {

    //SerializedProperty apparition;
    //SerializedProperty disparition;
    SerializedProperty question;
    SerializedProperty questionText;
    SerializedProperty questionImage;
    SerializedProperty questionAudio;
    SerializedProperty answer;
    SerializedProperty answerField;
    SerializedProperty answerButtons;
    SerializedProperty flashcard;

    void OnEnable()
    {
        //apparition = serializedObject.FindProperty("apparition");
        //disparition = serializedObject.FindProperty("disparition");
        question = serializedObject.FindProperty("question");
        questionText = serializedObject.FindProperty("questionText");
        questionImage = serializedObject.FindProperty("questionImage");
        questionAudio = serializedObject.FindProperty("questionAudio");
        answer = serializedObject.FindProperty("answer");
        answerField = serializedObject.FindProperty("answerField");
        answerButtons = serializedObject.FindProperty("answerButtons");
        flashcard = serializedObject.FindProperty("flashcard");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //EditorGUILayout.PropertyField(apparition);
        //EditorGUILayout.PropertyField(disparition);
        //EditorGUILayout.Space();
        EditorGUILayout.PropertyField(question);
        if (ValidCombination(false))
        {
            switch (question.enumValueIndex)
            {
                case 0:
                    EditorGUILayout.PropertyField(questionAudio);
                    break;
                case 1:
                    EditorGUILayout.PropertyField(questionText);
                    break;
                case 2:
                    EditorGUILayout.PropertyField(questionImage);
                    break;
                case 3:
                    EditorGUILayout.PropertyField(questionText);
                    break;
                default:
                    break;
            }

        }
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(answer);
        if (ValidCombination(true))
        {
            switch (answer.enumValueIndex)
            {
                case 0:
                    EditorGUILayout.PropertyField(answerField);
                    break;
                case 1:
                    EditorGUILayout.PropertyField(answerButtons, true);
                    break;
                case 2:
                    EditorGUILayout.PropertyField(answerField);
                    break;
                case 3:
                    EditorGUILayout.PropertyField(answerButtons, true);
                    break;
                case 4:
                    EditorGUILayout.PropertyField(answerButtons, true);
                    break;
                default:
                    break;
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(flashcard);
        serializedObject.ApplyModifiedProperties();
    }

    bool ValidCombination(bool log)
    {
        if (question.enumValueIndex == 1 && answer.enumValueIndex <= 1)
        {
            if (log)
            {
                EditorGUILayout.HelpBox("The answer is the same as the question.", MessageType.Warning);
            }
            return false;
        }
        if (question.enumValueIndex >= 2 && answer.enumValueIndex >= 2)
        {
            if (log)
            {
                EditorGUILayout.HelpBox("This exercice doesn’t include the learned language.", MessageType.Warning);
            }
            return false;
        }
        return true;
    }
}
