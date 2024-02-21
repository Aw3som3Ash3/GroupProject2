using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDetection : DuckDuckGoose
{ 
    public LocationManager locMan;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject curr = other.gameObject;
        if (curr.CompareTag("Player") && !curr.GetComponent<PlayerController>().crouched)
        {
            locMan.UpdateLocation(myLoc);
        }
    }
}
