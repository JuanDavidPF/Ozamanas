using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JuanPayan.Helpers
{
    public class SceneSwitcher : MonobehaviourEvents
    {
        [SerializeField] private string sceneToLoad;
        [SerializeField] private string sceneToBeActivated;

        [SerializeField] private LoadSceneMode mode;



        public override void Behaviour()
        {
            SceneManager.UnloadSceneAsync(sceneToLoad);

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, mode);
            if (sceneToBeActivated == sceneToLoad) loadOperation.allowSceneActivation = true;
        }//Closes Behaviour method



    }//Closes SceneSwitcher class
}//Closes namespace declaration