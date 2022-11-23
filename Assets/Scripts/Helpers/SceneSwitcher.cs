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

        public string SceneToLoad { get => sceneToLoad; set => sceneToLoad = value; }
        public LoadSceneMode Mode { get => mode; set => mode = value; }

        public override void Behaviour()
        {
            Debug.Log("Attempting to load scene: " + SceneToLoad);
            if (!IsAddeableToStack()) return;
            if (Mode == LoadSceneMode.Single) SceneStackManager.screenStack.Clear();

            SceneStackManager.screenStack.Add(SceneToLoad);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(SceneToLoad, Mode);
        }//Closes Behaviour method

        private bool IsAddeableToStack()
        {
            if (Mode == LoadSceneMode.Single) return true;


            if (SceneStackManager.screenStack.Contains(SceneToLoad)) return false;
            if (SceneManager.GetSceneByName(SceneToLoad).isLoaded) return false;
            return true;
        }//Closes IsAddeableToStack method

    }//Closes SceneSwitcher class
}//Closes namespace declaration