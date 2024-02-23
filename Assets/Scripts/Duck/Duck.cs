using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Duck : DuckDuckGoose
{
    public LocationManager locMan;

    private Rigidbody rb;

    private NavMeshAgent agent;

    public Transform goal;

    public bool know = false;

    public bool visible = false;

    public bool audible = false;
    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        locMan = GetComponent<LocationManager>();
        agent = GetComponent<NavMeshAgent>();

    }

    public void UpdateDestination(Transform player)
    {
        agent.destination = player.position;
        know = true;
    }
}
