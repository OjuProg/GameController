using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour {

    [SerializeField]
    private Engine engine;

    [SerializeField]
    [Range(0.0f, 1.0f)] private float dashThreshold;

    [SerializeField]
    private GameObject debugMenu;

    [SerializeField]
    private GameObject controlMenu;

    private DirectionsEnum.Direction recordedDirection;
    private float absLastSpeedRecorded;

    private void Start()
    {
        absLastSpeedRecorded = 0f;
    }

    // Update is called once per frame
    void Update () {

        if (this.engine != null)
        {   
            // Left / Right movements
            float horizontalSpeedPercent = Input.GetAxis("Horizontal");
            if(horizontalSpeedPercent == 0)
            {
                horizontalSpeedPercent = Input.GetAxis("CrossJoystickX");
            }

            if(Mathf.Abs(horizontalSpeedPercent) - absLastSpeedRecorded < 0.0f)
            {
                horizontalSpeedPercent = 0;
            }

            this.engine.Speed = new Vector2(horizontalSpeedPercent, 0);
            absLastSpeedRecorded = Mathf.Abs(horizontalSpeedPercent);

            // Recording the direction the player is facing according to the Dash Threshold.
            if (horizontalSpeedPercent >= dashThreshold)
            {
                recordedDirection = DirectionsEnum.Direction.Right;
            }
            if (horizontalSpeedPercent <= -dashThreshold)
            {
                recordedDirection = DirectionsEnum.Direction.Left;
            }

            // Sprint
            engine.isSprinting = (Input.GetButton("X") || Input.GetKeyDown(KeyCode.LeftShift));

            // Jump
            if(Input.GetButtonDown("A") || Input.GetKeyDown("space"))
            {
                this.engine.Jump();
            }

            // Dash
            if(Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(this.engine.Dash(recordedDirection));
            }

            // Debug
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(debugMenu != null)
                {
                    debugMenu.SetActive(!debugMenu.activeSelf);
                }
            }

            // Controls
            if (Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.Tab))
            {
                if (controlMenu != null)
                {
                    controlMenu.SetActive(!controlMenu.activeSelf);
                }
            }

            // Restart
            if(Input.GetButtonDown("Back") || Input.GetKeyDown(KeyCode.R))
            {
                int sceneIndex = SceneManager.GetActiveScene().buildIndex;
                if (SceneManager.GetSceneByBuildIndex(sceneIndex) != null)
                {
                    SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
                }
            }
        }
    }
}
