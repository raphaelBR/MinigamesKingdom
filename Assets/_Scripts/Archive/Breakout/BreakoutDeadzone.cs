using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutDeadzone : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BreakoutBall>() != null)
        {
            Debug.Log("Lost");
        }
    }
}
