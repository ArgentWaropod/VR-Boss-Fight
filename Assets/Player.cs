using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int health;
    public GameObject ded;

    private void Start()
    {
        ded.SetActive(false);
        health = 3;
    }


    public void DealDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            ded.SetActive(true);

        }
    }
}
