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
    public Animator animator;
    public GameObject sm;
    public AudioClip honk;
    public AudioSource audioSource;

    //Detection
    public bool onceQuacked;
    public float quackNormal;
    public float quackAccelerated;
    public float detectDistance = 30;
    
    //Vision
    public float radius;
    public float angle;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;
    
    //Patrol
    public float patrolDistance;
    public bool patrolling = true;
    public GameObject bread;

    public bool startPatrol;
    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
        StartCoroutine("DelayedVision");
    }

    // Update is called once per frame
    void Update()
    {
        if (startPatrol)
        {
            StartCoroutine("QuackTimer");
            BeginPatrol();
            patrolling = true;
            startPatrol = false;
            Debug.Log("Starting Patrol");
        }

        //SetCurrentSpeed();


        if (patrolling && locMan.broadcast)
        {
            patrolling = !locMan.broadcast;
            startPatrol = false;
        } else if (!patrolling && !locMan.broadcast)
        {
            patrolling = true;
            startPatrol = true;
        }
    }

    private void FixedUpdate()
    {
        animator.SetFloat("currentSpeed", agent.velocity.magnitude);
        Debug.Log(agent.velocity.magnitude);
    }

    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        //animator = sm.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void UpdateDestination(Transform player)
    {
        //Debug.Log("Trying to Update");
        if (Vector3.Distance(transform.position, playerObj.transform.position) < detectDistance)
        {
            //Debug.Log("Passed");
            agent.destination = player.position;
            Debug.Log("Updating");
        }
    }

    IEnumerator QuackTimer()
    {
        WaitForSeconds quackWait = onceQuacked ? new WaitForSeconds(quackAccelerated) : new WaitForSeconds(quackNormal);
        yield return quackWait;
        AreaCheck(false);
        audioSource.clip = honk;
        audioSource.Play();
        Debug.Log("Quacking");
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
            StartCoroutine("QuackTimer");
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
                    locMan.broadcast = true;
                    Debug.Log("Heard");

                }
            }
        }
        else if(rangeChecks.Length == 0 && !vision)
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
                //Debug.Log("Saw");
                locMan.broadcast = true;
                patrolling = false;
                Debug.Log("broadcast by vision");
                StopCoroutine("QuackTimer");
            }
            else
            {
                //Debug.Log("Obstructed");
            }
        }
    }

    IEnumerator DelayedVision()
    {
        while (true)
        {
            AreaCheck(true);
            yield return new WaitForSeconds(.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef.TakeDamage(1);
            playerRef.StartCoroutine("DamageDelay");
        }
    }

    /*void SetCurrentSpeed()
    {
        if (agent.velocity.magnitude >)
        {
            animator.SetFloat("currentSpeed", 1f);
        }
        else
        {
            animator.SetFloat("currentSpeed", 0f);
        }

        Debug.Log (rb.velocity.z);
        //Debug.Log(currentSpeed);
    }*/
}
