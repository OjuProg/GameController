using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Vector2 moveAmplitude;
    private Vector2 maxPosition;
    private Vector2 originPoint;

    private bool reversePath;
    public float speed;

    private void Start()
    {
        originPoint = gameObject.transform.position;
        maxPosition = originPoint + moveAmplitude;
        reversePath = false;
    }

    // Update is called once per frame
    void Update () {
        Vector2 curPosition = gameObject.transform.position;

        if((!reversePath && curPosition.x >= maxPosition.x && curPosition.y >= maxPosition.y) ||
            (reversePath && curPosition.x <= originPoint.x && curPosition.y <= originPoint.y))
        {
            reversePath = !reversePath;
            speed *= -1;
        }

        gameObject.transform.position = new Vector2(curPosition.x + moveAmplitude.x * speed * Time.deltaTime,
                                                    curPosition.y + moveAmplitude.y * speed * Time.deltaTime);
	}
}
