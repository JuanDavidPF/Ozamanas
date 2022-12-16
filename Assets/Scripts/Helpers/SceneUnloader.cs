using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JuanPayan.Helpers
{
    public class SceneUnloader : MonobehaviourEvents
    {
        [SerializeField] private string sceneToUnload;

        public string SceneToUnload { get => sceneToUnload; set => sceneToUnload = value; }

        public override void Behaviour()
        {
            Debug.Log("Attempting to unload scene: " + SceneToUnload);
            if (!SceneManager.GetSceneByName(SceneToUnload).isLoaded) return;

            SceneStackManager.screenStack.Remove(SceneToUnload);
            SceneManager.UnloadSceneAsync(SceneToUnload);

        }//Closes Behaviour method



    }//Closes SceneSwitcher class
}//Closes namespace declaration