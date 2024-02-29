using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDetection : DuckDuckGoose
{
    public LocationManager locMan;

    public Location boxLoc;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /*private void OnTriggerStay(Collider other)
    {
        GameObject curr = other.gameObject;
        if (curr.CompareTag("Player"))
        {
            Debug.Log(curr.GetComponent<PlayerController>().myLoc +" " + boxLoc);
            if (curr.GetComponent<PlayerController>().myLoc != boxLoc && !curr.GetComponent<PlayerController>().crouched)
            {
                locMan.UpdateLocation(myLoc);
            }
        }
    }*/
}