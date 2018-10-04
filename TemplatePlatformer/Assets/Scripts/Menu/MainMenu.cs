using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnChangeScene(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }

    public void OnChangeScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
