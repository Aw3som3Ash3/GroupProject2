using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bread : DuckDuckGoose
{
    public Vector3 breadOffset;
    
    public GameObject player;
    public GameObject enemy;
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
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (other.gameObject.CompareTag("Goose"))
        {
            enemy.GetComponent<Duck>().locMan.broadcast = false;
            enemy.GetComponent<Duck>().enabled = false;
            player.transform.GetChild(2).gameObject.SetActive(false);
            enemy.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gm.StartCoroutine("EndGame");
        }
        Debug.Log(other.gameObject.tag);
    }
}
