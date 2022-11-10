using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JuanPayan.Helpers
{
    public class SceneUnloader : MonobehaviourEvents
    {
        [SerializeField] private string sceneToUnload;


        public override void Behaviour()
        {
            Debug.Log("Attempting to unload scene: " + sceneToUnload);
            if (!SceneManager.GetSceneByName(sceneToUnload).isLoaded) return;

            SceneStackManager.screenStack.Remove(sceneToUnload);
            SceneManager.UnloadSceneAsync(sceneToUnload);

        }//Closes Behaviour method



    }//Closes SceneSwitcher class
}//Closes namespace declaration