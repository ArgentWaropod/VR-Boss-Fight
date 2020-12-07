using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States{ IDLE, SPIT, BITE, STOMP, RSTUN, LSTUN, CSTUN, DIE}

public class hydra : MonoBehaviour
{
    public Player playa;
    public GameObject Projectile, Raypoint;
    public Animator ani;
    public GameObject CHead, CHeadStump, RHead, RHeadStump, LHead, LHeadStump;
    public BoxCollider CHit, LHit, RHit, CSlice, LSlice, RSlice;
    public GameObject[] spitpoints;
    int LHeadHealth, RHeadHealth, CHeadHealth;
    bool isStumpBurned = false, inAttack = false, CHeadAlive = true, LHeadAlive = true, RHeadAlive = true;
    public bool stunned = false, isHeadCut = false, playerIsOnPlatform;
    States hydraStates;

    public void Start()
    {
        hydraStates = States.IDLE;
        CHead.SetActive(true);
        RHead.SetActive(true);
        LHead.SetActive(true);
        CHeadStump.SetActive(false);
        LHeadStump.SetActive(false);
        RHeadStump.SetActive(false);
        CHit.enabled = true;
        LHit.enabled = true;
        RHit.enabled = true;
        CSlice.enabled = false;
        LSlice.enabled = false;
        RSlice.enabled = false;
        LHeadHealth = 3;
        CHeadHealth = 3;
        RHeadHealth = 3;
        returnToIdle();
    }

    private void Update()
    {
        if (hydraStates != States.DIE && !inAttack)
        {
            if (hydraStates == States.CSTUN)
            {
                if (isHeadCut)
                {
                    CHead.SetActive(false);
                    CHeadStump.SetActive(true);
                }
            }
            else if (hydraStates == States.LSTUN)
            {
                if (isHeadCut)
                {
                    LHead.SetActive(false);
                    LHeadStump.SetActive(true);
                }
            }
            else if (hydraStates == States.RSTUN)
            {
                if (isHeadCut)
                {
                    RHead.SetActive(false);
                    RHeadStump.SetActive(true);
                }
            }
            else
            {
                if (playerIsOnPlatform)
                {
                    StartCoroutine("StompAttack");
                }
                else
                {
                    switch (randomizeAction())
                    {
                        case 0:
                            IdleAnimation(1);
                            break;
                        case 1:
                            IdleAnimation(2);
                            break;
                        case 2:
                            IdleAnimation(3);
                            break;
                        case 3:
                            hydraStates = States.BITE;
                            StartCoroutine("BiteAttack");
                            break;
                        case 4:
                            hydraStates = States.SPIT; 
                            StartCoroutine("SpitAttack");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public void BurnStump()
    {
        isStumpBurned = true;
    }
    public void DealDamage(int head)
    {
        switch (head)
        {
            case 0:
                CHeadHealth -= 1;
                if (CHeadHealth <= 0)
                {
                    hydraStates = States.CSTUN;
                    inAttack = false;
                }
                break;
            case 1:
                LHeadHealth -= 1;
                if (LHeadHealth <= 0)
                {
                    hydraStates = States.LSTUN;
                    inAttack = false;
                }
                break;
            case 2:
                RHeadHealth -= 1;
                if (RHeadHealth <= 0)
                {
                    hydraStates = States.RSTUN;
                    inAttack = false;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator SpitAttack()
    {
        inAttack = true;
        ani.SetTrigger("SpitAttack");
        yield return new WaitForSeconds(3.28f);
        for (int i = 0; i < spitpoints.Length; i++)
        {
            Instantiate(Projectile, spitpoints[i].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
        inAttack = false;
        yield return null;
    }
    
    public int randomizeAction()
    {
        int rand = Random.Range(0, 5);
        return rand;
    }

    IEnumerator BiteAttack()
    {
        inAttack = true;
        ani.SetTrigger("BiteAttack");
        yield return new WaitForSeconds(3.15f);

        inAttack = false;
        returnToIdle();
        yield return null;
    }

    IEnumerator StompAttack()
    {
        inAttack = true;
        ani.SetTrigger("StompAttack");
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < 3; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(Raypoint.transform.position, transform.TransformDirection(Vector3.forward), out hit, 10))
            {
                if (hit.collider.tag == "Player")
                {
                    playa.DealDamage();
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(.75f);
        inAttack = false;
        returnToIdle();
        yield return null;
    }

    public void returnToIdle()
    {
        hydraStates = States.IDLE;
        inAttack = false;
        isStumpBurned = false;
        CHit.enabled = true;
        LHit.enabled = true;
        RHit.enabled = true;
        CSlice.enabled = false;
        LSlice.enabled = false;
        RSlice.enabled = false;
        ani.SetTrigger("ReturnToIdle");
        ani.SetInteger("IdleRandomizer", Random.Range(1, 4));
    }

    public void Stun()
    {
        inAttack = true;
        CHit.enabled = false;
        LHit.enabled = false;
        RHit.enabled = false;
        CSlice.enabled = true;
        LSlice.enabled = true;
        RSlice.enabled = true;
        Invoke("Resolve", 15f);
    }

    public void Resolve()
    {
        if (hydraStates == States.CSTUN)
        {
            if (CHead.activeSelf == true)
            {
                ani.SetBool("IsHeadCut", false);
                ani.SetBool("IsStumpBurned", false);
                ani.SetTrigger("Resolving");
                Invoke("returnToIdle", 1.45f);
            }
            else if (CHead.activeSelf == false && isStumpBurned == false)
            {
                ani.SetBool("IsHeadCut", true);
                ani.SetBool("IsStumpBurned", false);
                ani.SetTrigger("Resolving");
                Invoke("returnToIdle", 1.983f);
            }
            else
            {
                if (!CHeadAlive && !LHeadAlive && !RHeadAlive)
                {
                    hydraStates = States.DIE;
                    ani.SetInteger("HeadsDead", 3);
                }
                else
                {
                    ani.SetBool("IsHeadCut", true);
                    ani.SetBool("IsStumpBurned", true);
                    ani.SetTrigger("Resolving");
                    CHeadAlive = false;
                    Invoke("returnToIdle", 1.983f);
                }
            }
        }
        else if (hydraStates == States.LSTUN)
        {
            if (LHead.activeSelf == true)
            {
                ani.SetBool("IsHeadCut", false);
                ani.SetBool("IsStumpBurned", false);
                ani.SetTrigger("Resolving");
                Invoke("returnToIdle", 1.45f);
            }
            else if (LHead.activeSelf == false && isStumpBurned == false)
            {
                ani.SetBool("IsHeadCut", true);
                ani.SetBool("IsStumpBurned", false);
                ani.SetTrigger("Resolving");
                Invoke("returnToIdle", 0.7f);
            }
            else
            {
                if (!CHeadAlive && !LHeadAlive && !RHeadAlive)
                {
                    hydraStates = States.DIE;
                    ani.SetInteger("HeadsDead", 3);
                }
                else
                {
                    ani.SetBool("IsHeadCut", true);
                    ani.SetBool("IsStumpBurned", true);
                    ani.SetTrigger("Resolving");
                    LHeadAlive = false;
                    Invoke("returnToIdle", 2.1f);
                }
            }
        }
        else if (hydraStates == States.RSTUN)
        {
            if (RHead.activeSelf == true)
            {
                ani.SetBool("IsHeadCut", false);
                ani.SetBool("IsStumpBurned", false);
                ani.SetTrigger("Resolving");
                Invoke("returnToIdle", 1.45f);
            }
            else if (RHead.activeSelf == false && isStumpBurned == false)
            {
                ani.SetBool("IsHeadCut", true);
                ani.SetBool("IsStumpBurned", false);
                ani.SetTrigger("Resolving");
                Invoke("returnToIdle", 2.217f);
            }
            else
            {
                if (!CHeadAlive && !LHeadAlive && !RHeadAlive)
                {
                    hydraStates = States.DIE;
                    ani.SetInteger("HeadsDead", 3);
                }
                else
                {
                    ani.SetBool("IsHeadCut", true);
                    ani.SetBool("IsStumpBurned", false);
                    ani.SetTrigger("Resolving");
                    RHeadAlive = false;
                    Invoke("returnToIdle", 2f);
                }
            }
        }
    }

    void IdleAnimation(int animation)
    {
        inAttack = true;
        if (animation == 1)
        {
            ani.SetInteger("IdleRandomizer", 1);
            Invoke("EndIdle", 5.033f);
        }
        else if (animation == 2)
        {
            ani.SetInteger("IdleRandomizer", 2);
            Invoke("EndIdle", 4.017f);
        }
        else if (animation == 3)
        {
            ani.SetInteger("IdleRandomizer", 3);
            Invoke("EndIdle", 5f);
        }
    }

    void EndIdle()
    {
        inAttack = false;
    }
}
