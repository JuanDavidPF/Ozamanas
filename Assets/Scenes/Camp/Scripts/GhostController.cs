using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
       Invoke("PlayIdleAnimation",Random.Range(0f, 3f));
    }

    private void PlayIdleAnimation()
    {

        animator.SetTrigger("Play");
    }

   
}
