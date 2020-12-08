using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void Update()
    {
        rb.velocity = transform.right * 3f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().DealDamage();

        }
        Destroy(gameObject);
    }
}
