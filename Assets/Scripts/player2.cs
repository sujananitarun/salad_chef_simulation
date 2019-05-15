using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2 : MonoBehaviour {

    // Use this for initialization
    public Rigidbody2D rb;
    public float speed = 5;

    void Move()
    {
        rb.velocity = new Vector2(Input.GetAxis("Fire2") * speed, rb.velocity.x);
        rb.velocity = new Vector2(Input.GetAxis("Fire1") * speed, rb.velocity.x);
    }

    void FixedUpdate () {
        Move();
    }
    
}
