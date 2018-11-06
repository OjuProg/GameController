using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private LevelManager levelManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        levelManager.LoadNextLevel();
    }
}
