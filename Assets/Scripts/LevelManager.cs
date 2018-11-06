using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Changes level when player collides with the EndOfLevel gameobject.
//The gameobject player needs to have the script Player with this script assigned to it.
public class LevelManager : MonoBehaviour {
    public void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.GetSceneByBuildIndex(sceneIndex + 1) != null)
        {
            SceneManager.LoadScene(sceneIndex+1, LoadSceneMode.Single);
        }
    }
}
