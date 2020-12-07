using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtbox : MonoBehaviour
{
    public int head;
    public hydra boss;

    private void OnTriggerEnter(Collider other)
    {
        boss.DealDamage(head);
    }
}
