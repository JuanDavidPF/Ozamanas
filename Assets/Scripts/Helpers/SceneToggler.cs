using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace JuanPayan.Helpers
{
    public class SceneToggler : MonobehaviourEvents
    {

        [SerializeField] private string sceneToLoad;


        public override void Behaviour()
        {
            Debug.Log("Attempting to toggle: " + sceneToLoad);
            if (!SceneStackManager.screenStack.Contains(sceneToLoad))
            {
                SceneStackManager.screenStack.Add(sceneToLoad);
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            }
            else
            {
                SceneStackManager.screenStack.Remove(sceneToLoad);
                AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(sceneToLoad);
            }

        }//Closes Behaviour method

        public void Behaviour(InputAction.CallbackContext context)
        {

            if (!context.performed) return;
            Debug.Log("Attempting to toggle: " + sceneToLoad);
            if (!SceneStackManager.screenStack.Contains(sceneToLoad))
            {
                SceneStackManager.screenStack.Add(sceneToLoad);
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            }
            else
            {
                SceneStackManager.screenStack.Remove(sceneToLoad);
                AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(sceneToLoad);
            }

        }//Closes Behaviour method



    }//Closes SceneSwitcher class
}//Closes namespace declaration