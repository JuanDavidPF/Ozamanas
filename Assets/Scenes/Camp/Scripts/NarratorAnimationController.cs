using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorAnimationController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator =GetComponentInChildren<Animator>();
    }

    public void StartTalking()
    {
       animator.SetInteger("Talking",Random.Range(1, 5));
    }

    public void StopTalking()
    {
        animator.SetTrigger("Idle");
        animator.SetInteger("Talking",0);
    }
   
}
