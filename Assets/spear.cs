using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spear : MonoBehaviour
{
    public bool hasFire = false;
    public GameObject fireEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fire")
        {
            hasFire = true;
            fireEffect.SetActive(true);
            Invoke("Douse", 10);
        }
    }

    private void Douse()
    {
        fireEffect.SetActive(false);
        hasFire = false;
    }
}
