using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class Zombie : MonoBehaviour
{
    public Action OnHealthChange;

    [Header("Move")]
    [SerializeField] float followDistance;
    [SerializeField] float attackDistance;

    [Header("Attack")]
    [SerializeField] float attackRate;
    [SerializeField] float damage;
    [SerializeField] float angleVision = 45;

    [Header("Health")]
    [SerializeField] bool isAlive;
    public float health;

    [SerializeField] GameObject healthPack;

    enum ZombieStates
    {
        STAND,
        MOVE,
        ATTACK
    }
    ZombieStates activeState;

    Animator anim;
    Player player;
    Rigidbody2D rb;
    ZombieMovement zombieMovement;
    AIPath path;

    public bool SetAlive()
    {
        return isAlive;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
        zombieMovement = GetComponent<ZombieMovement>();
        path = GetComponent<AIPath>();

        ChangeState(ZombieStates.STAND);
    }
    private void FixedUpdate()
    {
        UpdateState();
        
    }
    private void UpdateState()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        switch (activeState)
        {
            case ZombieStates.STAND:
                if (distance <= followDistance)
                {
                    if(CheckPlayer()<= angleVision)
                    {
                        ChangeStateToMove(distance);
                    }
                }
                anim.SetFloat("Speed",path.velocity.magnitude );
                break;
            case ZombieStates.MOVE:
                if (distance <= attackDistance && player.SetAlive())
                {
                    ChangeState(ZombieStates.ATTACK);
                }
                Rotate();
                if (distance >= followDistance)
                {
                    ChangeState(ZombieStates.STAND);
                }
                anim.SetFloat("Speed", path.velocity.magnitude);
                break;
            case ZombieStates.ATTACK:
                if (distance > attackDistance || player.SetAlive() == false)
                {
                    ChangeState(ZombieStates.MOVE);

                }
                Rotate();
                anim.SetFloat("Speed", path.velocity.magnitude);                
                break;
        }
    }

    private void ChangeStateToMove(float distance)
    {
        Vector2 direction = player.transform.position - transform.position;
        LayerMask layerMask = LayerMask.GetMask("Walls");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);
        if (hit.collider == null && player.SetAlive())
        {
            ChangeState(ZombieStates.MOVE);
        }
    }

    private void ChangeState(ZombieStates newState)
    {
        activeState = newState;
        switch (activeState)
        {
            case ZombieStates.STAND:
                zombieMovement.SetPatrol(true);
                break;
            case ZombieStates.MOVE:
                path.enabled = true;
                zombieMovement.SetAngry(false);
                zombieMovement.SetPatrol(false);
                StopAllCoroutines();
                break;
            case ZombieStates.ATTACK:
                zombieMovement.SetAngry(true);
                StartCoroutine(SetDamage(damage, attackRate));
                
                break;

        }
    }
    IEnumerator SetDamage(float damage, float rate)
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(rate);
            player.DoDamage(damage);
            anim.SetTrigger("Shoot");
        }
    }
    private void Rotate()
    {
        if(isAlive)
        {
            Vector3 direction = player.transform.position - transform.position;
            transform.up -= direction;
        }
       
    }
    public void DoDamage(float damage)
    {
        if (isAlive == true)
        {
            health -= damage;
            OnHealthChange();
            if (health <= 0)
            {
                isAlive = false;
                anim.SetBool("Dead", true);
                DropHealthPack();
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
    private void DropHealthPack()
    {
        int chanceToDrop = UnityEngine.Random.Range(0, 10);
        if(chanceToDrop >= 5)
        {
            Instantiate(healthPack, transform.position, Quaternion.identity);
        }
    }
    private float CheckPlayer()
    {
        Vector2 direction = player.transform.position - transform.position;
        Vector2 zombieVision = -transform.up;
        float vision = Vector2.Angle(zombieVision, direction);
        return vision;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followDistance);

        Gizmos.color = Color.magenta;
        Vector3 lookDirection = -transform.up;
        Vector3 v1 = Quaternion.AngleAxis(angleVision, Vector3.forward) * lookDirection;
        Vector3 v2 = Quaternion.AngleAxis(-angleVision, Vector3.forward) * lookDirection;
        Gizmos.DrawRay(transform.position, v1 * followDistance);
        Gizmos.DrawRay(transform.position, v2 * followDistance);
    }
}
