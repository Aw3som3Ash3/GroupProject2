using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : DuckDuckGoose
{
    private int health;
    private bool interactable = true;
    public float delayTiming;
    public virtual void TakeDamage(int damageAmnt)
    {
        if (interactable)
        {
            Debug.Log($"{this.gameObject.name} took {damageAmnt} points of damage");
            StartCoroutine("DamageDelay");
        }
    }

    IEnumerator DamageDelay()
    {
        WaitForSeconds wait = new WaitForSeconds(delayTiming);
        interactable = false;
        yield return wait;
        interactable = true;
    }
}
