using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakoutBall : MonoBehaviour {

    public float speed = 10f;

    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void Update()
    {
        rigid.velocity = rigid.velocity.normalized * speed;
    }
}
