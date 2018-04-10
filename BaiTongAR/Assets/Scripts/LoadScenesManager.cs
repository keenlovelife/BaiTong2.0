using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenesManager : MonoBehaviour {


    void Start () {
		
	}


    void Update () {
		
	}


    public void LoadScenes(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }
}
