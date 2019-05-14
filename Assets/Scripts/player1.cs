using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1 : MonoBehaviour {

    // Use this for initialization
    public Rigidbody2D rb;
    public float speed = 5;

    void Move()
    {
        
        rb.velocity = new Vector2(Input.GetAxis("Vertical") * speed, rb.velocity.y);
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.x);
    }

    void FixedUpdate () {
        Move();
    }
    
}
