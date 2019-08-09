﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutBrick : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BreakoutBall>() != null)
        {
            gameObject.SetActive(false);
        }
    }

}
