using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Lean.Pool;

public class Player : MonoBehaviour
{
    public Action OnPlayerHealth = delegate { };
    public Action OnPlayerDeath = delegate { };
    public Action OnBulletsCountChange = delegate { };


    [SerializeField] float fireRate;
    [SerializeField] float health;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPosition;
    [SerializeField] int bulletsCount;


    private float nextFire;
    private bool isAlive;
    

    Animator anim;
    SceneLoader sceneLoader;
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sceneLoader = FindObjectOfType<SceneLoader>();
        isAlive = true;
    }

    
    void Update()
    {
        Shoot();

    }
    public bool SetAlive()
    {
        return isAlive;
    }
    public float ReturnHealth()
    {
        return health;
    }
    public int ReturnBulletCount()
    {
        return bulletsCount;
    }

    public void DoDamage(float damage)
    {
        if (isAlive == true)
        {
            health -= damage;
            OnPlayerHealth();
            if (health <= 0)
            {
                isAlive = false;
                anim.SetBool("Dead", true);
                OnPlayerDeath();
                //StartCoroutine(RestartGame());
            }
        }
    }
    public void HealthUp(float index)
    {
        health += index;
        OnPlayerHealth();
    }
    public void AddBullets(int bullet)
    {
        bulletsCount += bullet;
        OnBulletsCountChange();

}

private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && nextFire <= 0 && isAlive == true)
        {
            if(bulletsCount != 0)
            {
                anim.SetTrigger("Shoot");
                LeanPool.Spawn(bullet, shootPosition.position, transform.rotation);
                nextFire = fireRate;
                bulletsCount--;
                OnBulletsCountChange();
            }
            
        }
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
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

    
    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3);
        anim.SetBool("Dead", false);
        sceneLoader.LoadActiveScene();


    }
   
}
