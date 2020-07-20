using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float fireRate = 1;

    public GameObject bullet;
    public Transform shootPosition;

    PlayerMovement player;
    Animator anim;

    private bool isAlive;

    void Start()
    {
        isAlive = true;
        player = FindObjectOfType<PlayerMovement>();
        anim = GetComponentInChildren<Animator>();

        StartCoroutine(Shoot(fireRate));
    }

    
    void Update()
    {
        RotateToPlayer();

    }
    public void DoDamage(float damage)
    {
        if(isAlive == true)
        {
            health -= damage;
            if (health <= 0)
            {
                isAlive = false;
                anim.SetBool("Dead", true);
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            DoDamage(damageDealer.damage);
        }
    }
    private void RotateToPlayer()
    {
        if(isAlive)
        {
            Vector3 playerTransform = player.transform.position;
            Vector3 direction = transform.position - playerTransform;
            transform.up += direction;
            
            
        }
        
    }

    IEnumerator Shoot(float delay)
    {
        
        while(isAlive)
        {
            yield return new WaitForSeconds(delay);
            Instantiate(bullet, shootPosition.position,transform.rotation);
            anim.SetTrigger("Shoot");
        }
        
    }

}
