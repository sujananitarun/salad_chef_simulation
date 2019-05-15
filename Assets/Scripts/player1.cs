using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player1 : MonoBehaviour
{

    // Use this for initialization
    public Rigidbody2D rb;
    public float speed = 5;

    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.collider.tag == "LetterMat a")
        {
            Debug.Log("a");
        }
        else if (col.collider.tag == "LetterMat b")
        {
            Debug.Log("b");
        }
        
    }

    

    void Move()
    {
            rb.velocity = new Vector2(Input.GetAxis("Vertical") * speed, rb.velocity.y);
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.x);
        
    }

    void FixedUpdate()
    {
        Move();
    }

}