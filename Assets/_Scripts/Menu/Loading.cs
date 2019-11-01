using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The canvas that transitions between scenes.
/// </summary>
public class Loading : MonoBehaviour {


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Reload();
        }
    }

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
