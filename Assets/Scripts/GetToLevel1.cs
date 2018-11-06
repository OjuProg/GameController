using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetToLevel1 : MonoBehaviour {

    // Update is called once per frame
    void Update ()
    {
		if(Input.anyKeyDown && Time.timeSinceLevelLoad > 1.0f)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
	}
}
