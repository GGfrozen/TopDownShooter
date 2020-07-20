using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditorInternal;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [Header("Patrol")]
    [SerializeField] bool isPatrol;
    [SerializeField] float patrolSpeed;
    [SerializeField] float minDistance;
    [SerializeField] Transform[] patrolPositions;


    Player player;
    Zombie zombie;
    Rigidbody2D rb;
    AIDestinationSetter destination;
    AIPath path;

    private int patrolIndex = 0;
    private bool isAngry;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        zombie = GetComponent<Zombie>();
        player = FindObjectOfType<Player>();
        destination = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        
    }
    private void FixedUpdate()
    {
        Moving();
    }
    private void Moving()
    {
        if (zombie.SetAlive())
        {
            if (!isAngry)
            {
                if (!isPatrol)
                {
                    MoveToPlayer();
                }
                else if (isPatrol)
                {
                    DoPatrol();
                }
            }
            if (isAngry)
            {
                StopMovement();
            }
        }
        else
        {
            StopMovement();
        }
    }

    private void MoveToPlayer()
    {
        //Vector3 direcrion = player.transform.position - transform.position;
        //rb.velocity = direcrion.normalized * moveSpeed;
        destination.target = player.transform;
    }

    private void DoPatrol()
    {
        Transform nextPosition = patrolPositions[patrolIndex];
        Vector2 moveToNextPosition = nextPosition.transform.position - transform.position;
        transform.up -= (Vector3)moveToNextPosition;
        //rb.velocity = moveToNextPosition.normalized * patrolSpeed;
        destination.target = nextPosition;
        float distance = Vector2.Distance(transform.position, nextPosition.transform.position);
        if (distance <= minDistance)
        {
            patrolIndex++;
            if (patrolIndex == patrolPositions.Length)
            {
                patrolIndex = 0;
            }
        }
    }
    private void StopMovement()
    {
        rb.velocity = Vector2.zero;
        path.enabled = false;
    }

    public void SetAngry(bool angry)
    {
        isAngry = angry;
    }
   public void SetPatrol(bool patrol)
    {
        isPatrol = patrol;
    }
}
