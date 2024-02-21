using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public LocationManager locMan;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitializeComponents(){
        rb = GetComponent<Rigidbody>();
        locMan = GetComponent<LocationManager>();
    }
}
