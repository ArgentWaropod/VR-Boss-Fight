using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slicebox : MonoBehaviour
{
    public hydra boss;

    private void OnTriggerEnter(Collider other)
    {
        boss.isHeadCut = true;
    }
}
