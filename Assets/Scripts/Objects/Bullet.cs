using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Lean.Pool;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        rb.velocity = -transform.up * speed;
    }
    private void OnBecameInvisible()
    {
        if(gameObject.activeSelf)
        {
            LeanPool.Despawn(gameObject);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LeanPool.Despawn(gameObject);
    }
}
