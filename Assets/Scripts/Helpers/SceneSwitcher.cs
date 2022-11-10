using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JuanPayan.Helpers
{
    public class SceneSwitcher : MonobehaviourEvents
    {

        [SerializeField] private string sceneToLoad;


        [SerializeField] private LoadSceneMode mode;



        public override void Behaviour()
        {
            Debug.Log("Attempting to load scene: " + sceneToLoad);
            if (!IsAddeableToStack()) return;
            if (mode == LoadSceneMode.Single) SceneStackManager.screenStack.Clear();

            SceneStackManager.screenStack.Add(sceneToLoad);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, mode);
        }//Closes Behaviour method

        private bool IsAddeableToStack()
        {
            if (mode == LoadSceneMode.Single) return true;


            if (SceneStackManager.screenStack.Contains(sceneToLoad)) return false;
            if (SceneManager.GetSceneByName(sceneToLoad).isLoaded) return false;
            return true;
        }//Closes IsAddeableToStack method

    }//Closes SceneSwitcher class
}//Closes namespace declaration