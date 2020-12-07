using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformAlert : MonoBehaviour
{
    public GameObject hydra;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hydra.GetComponent<hydra>().playerIsOnPlatform = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hydra.GetComponent<hydra>().playerIsOnPlatform = false;
        }
        
    }
}
