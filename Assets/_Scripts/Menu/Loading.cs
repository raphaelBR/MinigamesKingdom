using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    

    public void LoadScene(string levelName)
    {
        GetComponent<Animator>().SetBool("Load", true);
        SceneManager.LoadSceneAsync(levelName);
    }

    public void Reload()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

}
