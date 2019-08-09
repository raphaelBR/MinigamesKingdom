using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutPad : MonoBehaviour {

    public BreakoutBall basicBall;
    public Transform ballSpawnPoint;

    void Update()
    {
        if (Input.mousePresent)
        {
            transform.localPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y, transform.position.z);
        }
        if (Input.GetMouseButtonDown(0))
        {
            GameObject ball = Instantiate(basicBall.gameObject, ballSpawnPoint);
            ball.transform.parent = null;
            ball.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100f);
        }
    }

}
