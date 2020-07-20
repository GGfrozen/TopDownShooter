using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0.1f;

    Rigidbody2D rb;
    Animator anim;
    Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Rotate();

    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Rotate()
    {   
        if(player.SetAlive() == true)
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mouseWorldPosition - (Vector2)transform.position;
            transform.up = -direction;
        }
        
    }

    private void MovePlayer()
    {
        if (player.SetAlive() == true)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(moveX, moveY) * speed;

            anim.SetFloat("Speed", rb.velocity.magnitude);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
            
}
