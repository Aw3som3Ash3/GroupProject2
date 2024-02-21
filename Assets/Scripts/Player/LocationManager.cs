using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : DuckDuckGoose
{
    public Duck goose;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void UpdateLocation(Location now)
    {
        myLoc = now;
        goose.UpdateLocation(myLoc);
        Debug.Log(myLoc);
    }
    
}
