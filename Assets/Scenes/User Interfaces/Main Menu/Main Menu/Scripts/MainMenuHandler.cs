using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;


    private void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
    }//Closes Awake method

    private void Start()
    {

    }//Closes Start method

    void Update()
    {
        //if (Keyboard.current.anyKey.IsPressed()) SkipIntro();
    }//Closes Update method


    private void SkipIntro()
    {
        if (animator) animator.SetTrigger("Skip");
    }//Closes SkipIntro method


}//Closes MainMenuHandler method
