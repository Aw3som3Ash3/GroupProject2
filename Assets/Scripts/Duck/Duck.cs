using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Duck : Damageable
{
    //Components
    public LocationManager locMan;
    private Rigidbody rb;
    private NavMeshAgent agent;
    public GameObject playerObj;
    public PlayerController playerRef;

    //Detection
    public bool canSeePlayer;
    public bool canHearPlayer;
    public bool onceQuacked;
    public float quackNormal;
    public float quackAccelerated;
    public bool know;
    
    //Vision
    public float radius;
    public float angle;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;
    
    //Patrol
    public float patrolDistance;
    public bool patrolling;
    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        StartCoroutine("DelayedVision");
    }

    // Update is called once per frame
    void Update()
    {
        locMan.broadcast = canSeePlayer ? true : false;
        locMan.broadcast = canHearPlayer ? true : false;
        if (rb.velocity.magnitude < .1 && !patrolling)
        {
            StartCoroutine("QuackTimer");
            BeginPatrol();
            patrolling = true;
            Debug.Log("Starting Patrol");
        }
        if (know || canSeePlayer)
        {
            StopCoroutine("QuackTimer");
            locMan.broadcast = true;
        }
    }

    private void FixedUpdate()
    {
        
    }

    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        
    }

    public void UpdateDestination(Transform player)
    {
        Debug.Log("Trying to Update");
        if (Vector3.Distance(transform.position, playerObj.transform.position) < 15)
        {
            Debug.Log("Passed");
            agent.destination = player.position;
            know = true;
        }
    }

    IEnumerator QuackTimer()
    {
        WaitForSeconds quackWait = onceQuacked ? new WaitForSeconds(quackAccelerated) : new WaitForSeconds(quackNormal);
        yield return quackWait;
        patrolling = false;
        AreaCheck(false);
    }

    void BeginPatrol()
    {
        if (!patrolling)
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolDistance;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, patrolDistance, 1);
            Vector3 finalPosition = hit.position;
            agent.destination = finalPosition;
            patrolling = true;
        }
    }

    void AreaCheck(bool vision)
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetLayer);
        if (rangeChecks.Length != 0)
        {
            if (rangeChecks[0].gameObject == playerObj)
            {
                if (vision)
                {
                    FOVCheck(rangeChecks);
                }
                else if (playerRef.audible)
                {
                    onceQuacked = false;
                    canHearPlayer = true;
                    know = true;
                    patrolling = false;
                    Debug.Log("Heard");
                    BeginPatrol();
                }
            }
        }

        if (!canHearPlayer)
        {
            onceQuacked = true;
            StartCoroutine("QuackTimer");
        }
    }

    void FOVCheck(Collider[] rangeChecks)
    {
        Transform target = rangeChecks[0].transform;
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        if (Vector3.Angle(transform.position, directionToTarget) < angle/2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (!(Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleLayer)))
            {
                Debug.Log("Saw");
                canSeePlayer = true;
                locMan.broadcast = true;
            }
            else
            {
                canSeePlayer = false;
                Debug.Log("Obstructed");
            }
        }
    }

    IEnumerator DelayedVision()
    {
        while (true)
        {
            AreaCheck(true);
            yield return new WaitForSeconds(.1f);
            Debug.Log("Checking");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef.TakeDamage(1);
        }
    }
}
