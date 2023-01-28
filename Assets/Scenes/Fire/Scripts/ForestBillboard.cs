using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBillboard : MonoBehaviour
{

    private Camera mainCamera;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        if(TryGetComponent<Animator>(out animator)) Invoke("PlayIdleAnimation",Random.Range(0f,1f));
        
    }

    public void PlayIdleAnimation()
    {
        animator.SetTrigger("Idle");
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newRotation = mainCamera.transform.eulerAngles;
        newRotation.x = 0;
        newRotation.z = 0;
        transform.eulerAngles = newRotation;
    }
}
