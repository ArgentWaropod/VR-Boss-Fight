using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtbox : MonoBehaviour
{
    public int head;
    public hydra boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Spear" || other.tag == "Sword")
        {
            boss.DealDamage(head);
        }
    }
}
