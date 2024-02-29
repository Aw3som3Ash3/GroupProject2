using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : DuckDuckGoose
{
    public Duck goose;
    public Transform playerLoc;
    public bool broadcast = false;

    public GameObject UIObj;
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
        Debug.Log(now);
        if (now != Location.PlayerSpawn)
        {
            broadcast = true;
        }
        else
        {
            broadcast = false;
        }

        Debug.Log(broadcast);
    }

    public void UpdatePlayerLocation()
    {
        if (broadcast)
        {
            goose.UpdateDestination(playerLoc);
        }
    }
}