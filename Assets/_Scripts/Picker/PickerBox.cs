using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PickerBox : MonoBehaviour
{
    public Image img;
    public Text txt;
    public Button butn;
    public CustomAnimRect anim;
    public CustomAnimRect win;
    public CustomAnimRect error;
    public CustomAnimRect errorShake;
    public ParticleSystem particles;
    public ParticleSystem particlesItems;

    [HideInInspector]
    public string key;

    public void Init(string k)
    {
        img.sprite = Dico.Picture(k);
        txt.text = Dico.Locale(k);
        particlesItems.GetComponent<Renderer>().material.mainTexture = Dico.Picture(k).texture;
        key = k;
    }

    public void Close()
    {
        particles.Play();
        //particlesItems.Play();
        anim.GoTo(2);
        win.GoTo(1);
        butn.interactable = false;
        Destroy(this);
    }

    public void Error()
    {
        butn.interactable = false;
        error.GoTo(1);
        errorShake.PlayChain();
    }

    public void Clear()
    {
        butn.interactable = true;
        error.GoTo(0);
    }

}
