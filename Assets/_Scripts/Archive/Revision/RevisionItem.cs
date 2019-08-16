using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevisionItem : MonoBehaviour {

    public AudioSource sound;
    public Image picture;
    public Text locale;
    public Text foreign;

    public void Init(DicoItem i)
    {
        picture.sprite = i.picture;
        locale.text = i.localText;
        foreign.text = i.foreignText;
        sound.clip = i.foreignVoice;
    }
}
