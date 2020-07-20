using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float explosiveRadius;
    [SerializeField] float damage;

    Animator anim;

    private int delay = 1;

    private void Start()
    {
       
        anim = GetComponent<Animator>();
    }
    private void MakeExplode(float damage)
    {
        health -= damage;
        if (health<=0)
        {
            StartCoroutine(ExplodeBarrel(delay));
        }
    }
    private void DoExplosive()
    {
        LayerMask layerMask = LayerMask.GetMask("Player", "Enemy");
        Collider2D[] objectsInRadius = Physics2D.OverlapCircleAll(transform.position, explosiveRadius, layerMask);
        foreach (Collider2D objectsI in objectsInRadius)
        {
            if (objectsI.gameObject == gameObject)
            {
                continue;
            }
            Player player = objectsI.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.DoDamage(damage);
            }
            Enemy enemy = objectsI.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DoDamage(damage);
            }
            Zombie zombie = objectsI.gameObject.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.DoDamage(damage);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            MakeExplode(damageDealer.damage);
        }
    }
    IEnumerator ExplodeBarrel(float delay)
    {
        anim.SetTrigger("Boom");
        DoExplosive();
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosiveRadius);
    }
}
