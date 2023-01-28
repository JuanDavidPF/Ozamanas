using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ozamanas.Tags;

namespace JuanPayan.Helpers
{
    public class SceneSwitcher : MonobehaviourEvents
    {

        [SerializeField] private Scenes sceneToLoad;
        [SerializeField] private LoadSceneMode mode;
        public Scenes SceneToLoad { get => sceneToLoad; set => sceneToLoad = value; }
        public LoadSceneMode Mode { get => mode; set => mode = value; }

        public void SetSingleMode()
        {
            Mode = LoadSceneMode.Single;
        }

        public void SetAdditiveMode()
        {
            Mode = LoadSceneMode.Additive;
        }
        public override void Behaviour()
        {
            Debug.Log("Attempting to load scene: " + SceneToLoad);

            if (!IsAddeableToStack(SceneToLoad.ToString())) return;

            if (Mode == LoadSceneMode.Single) SceneStackManager.screenStack.Clear();

            SceneStackManager.screenStack.Add(SceneToLoad.ToString());

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(SceneToLoad.ToString(), Mode);
        }//Closes Behaviour method

        public void Behaviour(string sceneName)
        {
            Debug.Log("Attempting to load scene: " + sceneName);

            if (!IsAddeableToStack(sceneName)) return;

            if (Mode == LoadSceneMode.Single) SceneStackManager.screenStack.Clear();

            SceneStackManager.screenStack.Add(sceneName);

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, Mode);
        }

        private bool IsAddeableToStack(string sceneName)
        {
            if (Mode == LoadSceneMode.Single) return true;

            SceneStackManager.LoadScenesInList();

            if (SceneStackManager.screenStack.Contains(sceneName)) return false;
            
            if (SceneManager.GetSceneByName(sceneName).isLoaded) return false;

            return true;
        }//Closes IsAddeableToStack method

    }//Closes SceneSwitcher class
}//Closes namespace declaration