using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SoundManager
{
    public static SoundPlayer currentSound;

    public static void Play(SoundPlayer player)
    {
        if (currentSound != null)
        {
            currentSound.sound.Stop();
        }
        currentSound = player;
        if (currentSound != null)
        {
            currentSound.sound.Play();
        }
    }
}

public class SoundPlayer : MonoBehaviour {

    Button button;
    public AudioSource sound;
    AudioListener listener;
    public Image radialBar;

	// Use this for initialization
	void Start ()
    {
        button = GetComponent<Button>();
        radialBar.fillAmount = 0f;
	}
	
    public void Play()
    {
        StartCoroutine(Read());
    }

    IEnumerator Read()
    {
        SoundManager.Play(this);
        button.interactable = false;
        while (sound.isPlaying)
        {
            radialBar.fillAmount = sound.time / sound.clip.length;
            yield return null;
        }
        radialBar.fillAmount = 0f;
        button.interactable = true;
    }
}
