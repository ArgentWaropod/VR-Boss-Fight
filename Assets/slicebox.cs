using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slicebox : MonoBehaviour
{
    public hydra boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            Debug.Log("Slice and Dice");
            boss.isHeadCut = true;
        }
        else if (other.tag == "Spear")
        {
            if (other.gameObject.GetComponent<spear>().hasFire)
            {
                Debug.Log("Toasty");
                boss.BurnStump();
            }
        }
    }
}
