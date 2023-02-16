using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Ozamanas.GameScenes
{

public class FullScreenController : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    private bool fullScreen;

    public bool FullScreen
    {
     get { return fullScreen; }
     private set { fullScreen = value; }
    }

    void Start()
    {
        if (Screen.fullScreen) toggle.isOn = true;
        else toggle.isOn = false;
    }

    public void ActivateFullScreen(bool value)
    {
        Screen.fullScreen = value;
        fullScreen = value;
    }

}
}
